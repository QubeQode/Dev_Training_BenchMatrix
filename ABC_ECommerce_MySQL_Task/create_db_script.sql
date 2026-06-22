CREATE DATABASE abc_ecommerce;
USE abc_ecommerce;

CREATE TABLE Customers
(
	customer_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    registration_date DATE NOT NULL
);

CREATE TABLE Categories
(
	category_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    category_name VARCHAR(100) NOT NULL,
    parent_category_id INT NULL,
    
    FOREIGN KEY (parent_category_id)
		REFERENCES Categories(category_id)
);

CREATE TABLE Products
(
	product_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    category_id INT NOT NULL,
    product_name VARCHAR(100) NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    stock_quantity INT NOT NULL,
    
    FOREIGN KEY (category_id)
		REFERENCES Categories(category_id)
);

CREATE TABLE Orders
(
	order_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    customer_id INT NOT NULL,
    order_date DATE NOT NULL,
    
    FOREIGN KEY (customer_id)
		REFERENCES Customers(customer_id)
);

CREATE TABLE OrderItems
(
	order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    
    PRIMARY KEY(order_id, product_id),
    
    FOREIGN KEY (order_id)
		REFERENCES Orders(order_id),
        
	FOREIGN KEY (product_id)
		REFERENCES Products(product_id)
);

CREATE TABLE Payments
(
	payment_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    order_id INT NOT NULL,
    payment_date DATE NULL,
    amount DECIMAL(10, 2) NOT NULL,
    payment_status VARCHAR(20) NOT NULL,
    
    FOREIGN KEY (order_id)
		REFERENCES Orders(order_id)
);

CREATE TABLE RestockAlerts
(
	alert_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    product_id INT NOT NULL,
    current_stock INT NOT NULL,
    alert_date DATETIME NOT NULL,
    
    FOREIGN KEY (product_id)
		REFERENCES Products(product_id)
);

CREATE TABLE MonthlySalesAudit
(
	audit_id INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    audit_month INT NOT NULL,
    audit_year INT NOT NULL,
    total_orders INT NOT NULL,
    total_revenue DECIMAL(12, 2) NOT NULL,
    created_date DATETIME NOT NULL
);
