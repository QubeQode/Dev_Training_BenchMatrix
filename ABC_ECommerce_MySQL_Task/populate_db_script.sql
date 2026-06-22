INSERT INTO Customers
(first_name, last_name, registration_date)
VALUES
('John', 'Smith', '2024-01-10'),
('Sarah', 'Jones', '2023-02-01'),
('Mike', 'Brown', '2024-03-15'),
('Emma', 'Wilson', '2024-04-01'),
('David', 'Taylor', '2024-05-20');

INSERT INTO Categories
(category_name, parent_category_id)
VALUES
('Electronics', NULL),
('Home Appliances', NULL),
('Phones', 1),
('Smartphones', 3),
('Home Phones', 3),
('Computers', 1),
('Laptops', 6),
('Desktops', 6),
('Stovetops', 2);

INSERT INTO Products
(category_id, product_name, unit_price, stock_quantity)
VALUES
(4, 'iPhone 15', 1200, 25),
(4, 'Samsung S24', 1100, 15),
(7, 'Dell XPS 15', 2000, 8),
(7, 'MacBook Pro', 3000, 12),
(9, 'KitchenAid Induction Stove', 1600, 8);

INSERT INTO Orders
(customer_id, order_date)
VALUES
(1, '2026-01-10'),
(1, '2026-02-15'),
(2, '2026-02-18'),
(2, '2026-02-20'),
(3, '2026-02-22'),
(5, '2026-03-01'),
(5, '2026-03-11');

INSERT INTO OrderItems
(order_id, product_id, quantity, unit_price)
VALUES
(1, 1, 1, 1000),
(1, 5, 1, 1600),
(2, 2, 1, 1100),
(3, 4, 1, 3000),
(4, 3, 2, 2000),
(5, 1, 2, 1200),
(6, 5, 1, 1600),
(7, 4, 1, 3000);

INSERT INTO Payments
(order_id, payment_date, amount, payment_status)
VALUES
(1, '2026-01-10', 960, 'Paid'),
(2, '2026-02-15', 1100, 'Paid'),
(3, NULL, 3000, 'Pending'),
(4, '2026-03-02', 4000, 'Paid'),
(5, NULL, 2400, 'Pending'),
(6, '2026-03-01', 1600, 'Paid'),
(7, '2026-03-11', 3000, 'Paid');
