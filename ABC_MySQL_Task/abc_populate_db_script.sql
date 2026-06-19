INSERT INTO Departments(department_name)
VALUES
('IT'),
('Finance'),
('HR'),
('Marketing');

INSERT INTO SalaryTypes(salary_type_name)
VALUES
('Monthly'),
('Annual');

INSERT INTO Employees
(first_name, last_name, hire_date, department_id, manager_id)
VALUES
('John', 'Smith', '2018-01-15', 1, NULL),
('Mary', 'Brown', '2019-03-10', 2, 1),
('David', 'Lee', '2020-05-20', 2, 2),
('Sarah', 'Jones', '2021-07-01', 3, 1),
('Michael', 'White', '2022-09-15', 4, 1);

INSERT INTO Salaries
(employee_id, salary_amount, salary_type_id, effective_date)
VALUES
(1, 120000, 2, '2025-01-01'),
(2, 8500, 1, '2025-01-01'),
(3, 95000, 2, '2025-01-01'),
(4, 7000, 1, '2025-01-01'),
(5, 9000, 1, '2025-01-01');

INSERT INTO Projects
(project_name, start_date, end_date, project_status)
VALUES
('ERP Upgrade', '2025-01-01', NULL, 'Active'),
('Mobile App', '2025-01-01', '2025-04-15', 'Complete'),
('Payroll System', '2025-03-01', NULL, 'Active'),
('Time Tracker', '2025-04-01', NULL, 'Active');

INSERT INTO EmployeeProjects
(employee_id, project_id, hours_logged)
VALUES
(1, 1, 150),
(1, 2, 80),
(1, 3, 15),
(2, 3, 120),
(3, 3, 110),
(4, 3, 90),
(4, 4, 150);

SELECT COUNT(*) FROM Departments;
SELECT COUNT(*) FROM Employees;
SELECT COUNT(*) FROM Salaries;
SELECT COUNT(*) FROM Projects;
SELECT COUNT(*) FROM EmployeeProjects;
