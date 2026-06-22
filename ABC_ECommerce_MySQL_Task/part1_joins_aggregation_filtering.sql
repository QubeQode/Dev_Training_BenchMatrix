-- Customer Total Orders, Total Expenditure
SELECT
	c.customer_id,
    CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
    COUNT(DISTINCT o.order_id) AS total_orders,
    COALESCE(SUM(p.amount), 0) AS total_amount_spent
FROM Customers c
LEFT JOIN Orders o
	ON c.customer_id = o.customer_id
LEFT JOIN Payments p
	ON o.order_id = p.order_id
GROUP BY
	c.customer_id,
    c.first_name,
    c.last_name
ORDER BY c.customer_id;

-- Customer with More Than Three Orders + At Least One Unpaid Order
SELECT
	CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
    COUNT(o.order_id) AS total_orders
FROM Customers c
INNER JOIN Orders o
	ON c.customer_id = o.customer_id
GROUP BY
	c.customer_id,
    c.first_name,
    c.last_name
HAVING
	COUNT(o.order_id) > 3
    AND EXISTS
    (
		SELECT 1
        FROM Orders o2
        INNER JOIN Payments p
			ON o2.order_id = p.order_id
		WHERE o2.customer_id = c.customer_id
			AND p.payment_status <> 'Paid'
    );

-- Distinct Customers, Total Sold, Total Revenue p/ Product
SELECT
	p.product_id,
    p.product_name,
    COUNT(DISTINCT o.customer_id) AS distinct_customers,
    SUM(oi.quantity) AS total_units_sold,
    SUM(oi.quantity * oi.unit_price) AS total_revenue
FROM Products p
INNER JOIN OrderItems oi
	ON p.product_id = oi.product_id
INNER JOIN Orders o
	ON oi.order_id = o.order_id
GROUP BY
	p.product_id,
    p.product_name
ORDER BY total_revenue DESC;

-- Show Orders Alongside Payment Status + DATEDIFF From Order Date
SELECT
	o.order_id,
    o.order_date,
    p.payment_status,
    
    CASE
		WHEN p.payment_date is NULL
			THEN NULL
		ELSE DATEDIFF(p.payment_date, o.order_date)
	END AS days_to_payment

FROM Orders o
INNER JOIN Payments p
	ON o.order_id = p.order_id
ORDER BY o.order_id;

-- Top Three Bestselling Products p/ Product Category by Units Sold
WITH ProductSales AS
(
	SELECT
		c.category_id,
        c.category_name,
        p.product_id,
        p.product_name,
        SUM(oi.quantity) AS units_sold
	FROM Categories c
    INNER JOIN Products p
		ON c.category_id = p.category_id
	INNER JOIN OrderItems oi
		ON p.product_id = oi.product_id
	GROUP BY
		c.category_id,
        c.category_name,
        p.product_id,
        p.product_name
),
RankedProducts AS
(
	SELECT
		*,
        ROW_NUMBER() OVER
        (
			PARTITION BY category_id
            ORDER BY units_sold DESC
        ) AS product_rank
	FROM ProductSales
)
SELECT
	category_name,
    product_name,
    units_sold,
    product_rank
FROM RankedProducts
WHERE product_rank <= 3
ORDER BY
	category_name,
    product_rank;

-- Compare Customer Spending Against Average
WITH CustomerTotals AS
(
	SELECT
		c.customer_id,
        CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
        COALESCE(SUM(p.amount), 0) AS total_spent
	FROM Customers c
    LEFT JOIN Orders o
		ON c.customer_id = o.customer_id
	LEFT JOIN Payments p
		ON o.order_id = p.order_id
		AND p.payment_status = 'Paid'
	GROUP BY
		c.customer_id,
        c.first_name,
        c.last_name
)
SELECT
	customer_name,
    total_spent,
    
    CASE
		WHEN total_spent >
			(SELECT AVG(total_spent) FROM CustomerTotals)
		THEN 'Above Average'
        
        WHEN total_spent <
			(SELECT AVG(total_spent) FROM CustomerTotals)
		THEN 'Below Average'
        
        ELSE 'Average'
	END AS spending_category

FROM CustomerTotals
ORDER BY total_spent DESC;

-- Monthly Revenue for A Year + Running Cumulative Total p/ Month
WITH MonthlyRevenue AS
(
	SELECT
		DATE_FORMAT(payment_date, '%Y-%m') AS revenue_month,
        SUM(amount) AS monthly_revenue
	FROM Payments
    WHERE payment_status = 'Paid'
		AND payment_date >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH)
	GROUP BY
		DATE_FORMAT(payment_date, '%Y-%m')
)
SELECT
	revenue_month,
    monthly_revenue,
    
    SUM(monthly_revenue)
    OVER
    (
		ORDER BY revenue_month
    ) AS running_total

FROM MonthlyRevenue
ORDER BY revenue_month;
