-- Scalar Function: Customer Lifetime Value
DELIMITER $$
CREATE FUNCTION fn_customer_lifetime_value
(
	p_customer_id INT
)
RETURNS DECIMAL(12, 2)
DETERMINISTIC

BEGIN
	DECLARE v_total_spent DECIMAL(12, 2);
    
    SELECT COALESCE(SUM(p.amount), 0)
    INTO v_total_spent
    FROM Orders o
    INNER JOIN Payments p
		ON o.order_id = p.order_id
	WHERE o.customer_id = p_customer_id
		AND p.payment_status = 'Paid';
	
    RETURN v_total_spent;
END $$
DELIMITER ;

-- Test
SELECT fn_customer_lifetime_value(1); -- Expected 2060
SELECT fn_customer_lifetime_value(3); -- Expected 0

-- Scalar Function: Apply Discount on Order
DELIMITER $$
CREATE FUNCTION fn_order_discount
(
	p_order_id INT
)
RETURNS DECIMAL(12, 2)
DETERMINISTIC

BEGIN
	DECLARE v_order_total DECIMAL(12, 2);
    DECLARE v_discounted_total DECIMAL(12, 2);
	
    SELECT SUM(quantity * unit_price)
    INTO v_order_total
    FROM OrderItems
    WHERE order_id = p_order_id;
    
    IF v_order_total > 10000 THEN
		SET v_discounted_total = v_order_total * 0.9;
	ELSEIF v_order_total > 5000 THEN
		SET v_discounted_total = v_order_total * 0.95;
	ELSE
		SET v_discounted_total = v_order_total;
	END IF;
    
    RETURN v_discounted_total;
END $$
DELIMITER ;

-- Test
SELECT 
	order_id, 
    fn_order_discount(order_id) AS discounted_total
FROM Orders;

-- Inline TVF for All Orders in a Specific Range
/*
Since TVF functions and CROSS APPLY are SQL syntax and not available in MySQL I will create a
PROCEDURE that achieves the same thing, taking in a start and end date and using SELECT to display
all orders as an output.
*/
DELIMITER $$
CREATE PROCEDURE fn_orders_by_date_range
(
	IN p_start_date DATE,
    IN p_end_date DATE
)
BEGIN
	SELECT
		o.order_id,
        CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
        o.order_date,
        SUM(oi.quantity) AS total_items,
        SUM(oi.quantity * oi.unit_price) AS order_total
	FROM Orders o
    JOIN Customers c
		ON o.customer_id = c.customer_id
	JOIN OrderItems oi
		ON o.order_id = oi.order_id
	WHERE o.order_date BETWEEN p_start_date AND p_end_date
    GROUP BY
		o.order_id,
        customer_name,
        o.order_date;
END $$
DELIMITER ;

-- Test
CALL fn_orders_by_date_range ('2026-02-01', '2026-02-28');

-- Order Placement Procedure
DELIMITER $$
CREATE PROCEDURE sp_place_order
(
	IN p_customer_id INT,
    IN p_product_id INT,
    IN p_quantity INT
)
BEGIN
	DECLARE v_order_id INT;
    DECLARE v_unit_price DECIMAL(10, 2);
    DECLARE v_stock INT;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
		ROLLBACK;
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Order placement failed. Transaction rolled back.';
	END;
    
    START TRANSACTION;
    SELECT
		stock_quantity,
        unit_price
	INTO
		v_stock,
        v_unit_price
	FROM Products
	WHERE product_id = p_product_id;
    
    IF v_stock < p_quantity THEN
		SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Insufficient stock';
	END IF;
    
    INSERT INTO Orders
    (
		customer_id,
        order_date
    )
    VALUES
    (
		p_customer_id,
        CURDATE()
    );
    
    SET v_order_id = LAST_INSERT_ID();
    
    INSERT INTO OrderItems
    (
		order_id,
        product_id,
        quantity,
        unit_price
    )
    VALUES
    (
		v_order_id,
        p_product_id,
        p_quantity,
        v_unit_price
    );
    
    UPDATE Products
    SET stock_quantity = stock_quantity - p_quantity
	WHERE product_id = p_product_id;
    
    INSERT INTO Payments
    (
		order_id,
        payment_date,
        amount,
        payment_status
    )
    VALUES
    (
		v_order_id,
        NULL,
        p_quantity * v_unit_price,
        'Pending'
    );
    
    COMMIT;
END $$
DELIMITER ;

-- Test
CALL sp_place_order(1, 2, 1); -- SUCCESS
CALL sp_place_order(1, 3, 999); -- FAIL

-- Monthly Sales Report
DELIMITER $$
CREATE PROCEDURE sp_monthly_sales_report
(
	IN p_year INT,
    IN p_month INT,
    OUT p_total_orders INT,
    OUT p_total_revenue DECIMAL(12, 2)
)
BEGIN
	SELECT COUNT(*)
    INTO p_total_orders
    FROM Orders
	WHERE YEAR(order_date) = p_year
		AND MONTH(order_date) = p_month;
	
    SELECT COALESCE(SUM(amount), 0)
    INTO p_total_revenue
    FROM Payments
    WHERE payment_status = 'Paid'
		AND YEAR(payment_date) = p_year
        AND MONTH(payment_date) = p_month;
	
    -- Top 5 Products
    SELECT
		p.product_name,
        SUM(oi.quantity) AS units_sold
	FROM OrderItems oi
	INNER JOIN Products p
		ON oi.product_id = p.product_id
	INNER JOIN Orders o
		ON oi.order_id = o.order_id
	WHERE YEAR(o.order_date) = p_year
		AND MONTH(o.order_date) = p_month
	GROUP BY p.product_name
    ORDER BY units_sold DESC
    LIMIT 5;
    
    -- Top Three Customers
    SELECT
		CONCAT(c.first_name, ' ', c.last_name)
			AS customer_name,
		SUM(p.amount) AS spending
	FROM Customers c
    INNER JOIN Orders o
		ON c.customer_id = o.customer_id
	INNER JOIN Payments p
		ON o.order_id = p.order_id
	WHERE p.payment_status = 'Paid'
		AND YEAR(p.payment_date) = p_year
        AND MONTH(p.payment_date) = p_month
	GROUP BY customer_name
    ORDER BY spending DESC
    LIMIT 3;
END $$
DELIMITER ;

DROP PROCEDURE sp_monthly_sales_report;

-- Test
CALL sp_monthly_sales_report(
	2026,
    2,
    @orders,
    @revenue
);

SELECT
	@orders AS total_orders,
    @revenue AS total_revenue;

-- Cursor for Create Restock Alerts
DELIMITER $$
CREATE PROCEDURE sp_generate_restock_alerts()
BEGIN
	DECLARE done INT DEFAULT FALSE;
    DECLARE v_product_id INT;
    DECLARE v_stock INT;
    DECLARE low_stock_cursor CURSOR FOR
		SELECT
			product_id,
            stock_quantity
		FROM Products
        WHERE stock_quantity < 10;
	
    DECLARE CONTINUE HANDLER
		FOR NOT FOUND
        SET done = TRUE;
	
    OPEN low_stock_cursor;
    
    read_loop: LOOP
		FETCH low_stock_cursor
        INTO v_product_id, v_stock;
        
        IF done THEN
			LEAVE read_loop;
		END IF;
        
        INSERT INTO RestockAlerts
        (
			product_id,
            current_stock,
            alert_date
        )
        VALUES
        (
			v_product_id,
            v_stock,
            NOW()
        );
	END LOOP;
    
    CLOSE low_stock_cursor;
END $$
DELIMITER ;

-- Test
CALL sp_generate_restock_alerts();
SELECT * FROM RestockAlerts;

-- Cursor for Monthly Audit Builder
DELIMITER $$
CREATE PROCEDURE sp_build_sales_audit()
BEGIN
	DECLARE done INT DEFAULT FALSE;
    DECLARE v_month INT;
    DECLARE v_year INT;
    DECLARE v_orders INT;
    DECLARE v_revenue DECIMAL(12, 2);
    DECLARE month_cursor CURSOR FOR
		WITH RECURSIVE month_list AS
        (
			SELECT
				YEAR(CURDATE()) AS report_year,
                MONTH(CURDATE()) AS report_month,
                0 AS counter
			
            UNION ALL
            
            SELECT
				YEAR(DATE_SUB(CURDATE(), INTERVAL counter + 1 MONTH)),
                MONTH(DATE_SUB(CURDATE(), INTERVAL counter + 1 MONTH)),
                counter + 1
			FROM month_list
            WHERE counter < 11
        )
        SELECT
			report_month,
            report_year
		FROM month_list;
        
        DECLARE CONTINUE handler
			FOR NOT FOUND
            SET done = TRUE;
		
        OPEN month_cursor;
        
        month_loop: LOOP
			FETCH month_cursor
            INTO v_month, v_year;
		
			IF done THEN
				LEAVE month_loop;
			END IF;
            
            CALL sp_monthly_sales_report(
				v_year,
                v_month,
                @orders,
                @revenue
            );
			
			SET v_orders = COALESCE(@orders, 0);
			SET v_revenue = COALESCE(@revenue, 0);
			
			INSERT INTO MonthlySalesAudit
			(
				audit_month,
				audit_year,
				total_orders,
				total_revenue,
				created_date
			)
			VALUES
			(
				v_month,
				v_year,
				v_orders,
				v_revenue,
				NOW()
			);
		END LOOP;
        
		CLOSE month_cursor;
END $$
DELIMITER ;

-- Test
CALL sp_build_sales_audit();
SELECT * FROM MonthlySalesAudit;
