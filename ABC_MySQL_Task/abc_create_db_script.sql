CREATE DATABASE abc_company;
USE abc_company;

CREATE TABLE Departments
(
	department_id INT AUTO_INCREMENT PRIMARY KEY,
    department_name VARCHAR(100) NOT NULL
);

CREATE TABLE Employees
(
	employee_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    hire_date DATE,
    department_id INT,
    manager_id INT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    
    FOREIGN KEY(department_id)
		REFERENCES Departments(department_id),
	
    FOREIGN KEY(manager_id)
		REFERENCES Employees(employee_id)
);

CREATE TABLE SalaryTypes
(
	salary_type_id INT AUTO_INCREMENT PRIMARY KEY,
    salary_type_name VARCHAR(20)
);

CREATE TABLE Salaries
(
	salary_id INT AUTO_INCREMENT PRIMARY KEY,
    employee_id INT,
    salary_amount DECIMAL (12,2),
    salary_type_id INT,
    effective_date DATE,
    
    FOREIGN KEY(employee_id)
		REFERENCES Employees(employee_id),
	
    FOREIGN KEY(salary_type_id)
		REFERENCES SalaryTypes(salary_type_id)
);

CREATE TABLE Projects
(
	project_id INT AUTO_INCREMENT PRIMARY KEY,
    project_name VARCHAR(100),
    start_date DATE,
    end_date DATE,
    project_status VARCHAR(20)
);

CREATE TABLE EmployeeProjects
(
	employee_id INT,
    project_id INT,
    hours_logged DECIMAL(10,2),
    
    PRIMARY KEY(employee_id, project_id),
    
    FOREIGN KEY(employee_id)
		REFERENCES Employees(employee_id),
	
    FOREIGN KEY(project_id)
		REFERENCES Projects(project_id)
);

SHOW TABLES;

DESCRIBE Departments;
DESCRIBE Employees;
DESCRIBE Salaries;
DESCRIBE Projects;
DESCRIBE EmployeeProjects;
