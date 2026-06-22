-- Customers Who Ordered Within 7 Days of Signing Up
WITH FirstOrders AS
(
	SELECT
		customer_id,
        MIN(order_date) AS first_order_date
	FROM Orders
    GROUP BY customer_id
)
SELECT
	CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
    c.registration_date,
    fo.first_order_date,
    DATEDIFF(fo.first_order_date, c.registration_date) AS days_between
FROM Customers c
JOIN FirstOrders fo
	ON c.customer_id = fo.customer_id
WHERE DATEDIFF(fo.first_order_date, c.registration_date) <= 7;

-- Recursive CTE for Product Category Heirarchy
WITH RECURSIVE CategoryHeirarchy AS
(
	SELECT
		category_id,
        category_name,
        parent_category_id,
        category_name AS category_path
	FROM Categories
    WHERE parent_category_id IS NULL
    
    UNION ALL
    
    SELECT
		c.category_id,
        c.category_name,
        c.parent_category_id,
        CONCAT(ch.category_path, ' > ', c.category_name) 
			AS category_path
	FROM Categories c
    INNER JOIN CategoryHeirarchy ch
		ON c.parent_category_id = ch.category_id
)
SELECT
	category_id,
    category_name,
    category_path
FROM CategoryHeirarchy
ORDER BY category_path;

-- View of Customer Names, Total Orders, Total Spent + Last Order
CREATE VIEW vw_customer_order_summary AS
SELECT
	c.customer_id,
    CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
    COUNT(DISTINCT o.order_id) AS total_orders,
    COALESCE(SUM(p.amount), 0) AS total_amount_spent,
    MAX(o.order_date) AS most_recent_order
FROM Customers c
LEFT JOIN Orders o
	ON c.customer_id = o.customer_id
LEFT JOIN Payments p
	ON o.order_id = p.order_id
GROUP BY
	c.customer_id,
    customer_name;

SELECT * FROM vw_customer_order_summary;

-- Index Solution: Product Revenue Report
EXPLAIN
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

/*
Index I would have created:
CREATE INDEX idx_orderItems_product_order
ON OrderItems(product_id, order_id);
/*

/*
BEFORE INDEX
-> Sort: total_revenue DESC\n    
-> Stream results\n        
-> Group aggregate: count(distinct orders.customer_id), 
	sum(orderitems.quantity), sum(tmp_field)
-> Sort: p.product_id, p.product_name
-> Stream results  (cost=6.3 rows=8)
-> Nested loop inner join  (cost=6.3 rows=8)
-> Nested loop inner join  (cost=3.5 rows=8)
-> Covering index scan on o using customer_id  (cost=0.95 rows=7)
-> Index lookup on oi using PRIMARY (order_id = o.order_id)  
	(cost=0.266 rows=1.14)
-> Single-row index lookup on p using PRIMARY 
	(product_id = oi.product_id)  (cost=0.262 rows=1)
*/

SHOW INDEX FROM OrderItems;

/*
Why I didn't implement an index:
Execution plan shows that usage of composite key (order_id, product_id) and foreign key notation
implicitly creates indexes that support the JOINs in the product revenue query. Since the required
indexes already exist I didn't create any further indexes.
*/
