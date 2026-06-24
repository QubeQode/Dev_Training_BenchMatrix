-- Employee, Department, Salary
SELECT
	e.employee_id,
    CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    d.department_name,
    s.salary_amount
FROM Employees e
LEFT JOIN Departments d
	ON e.department_id = d.department_id
LEFT JOIN Salaries s
	ON e.employee_id = s.employee_id;

-- Department Headcount
SELECT
	d.department_name,
    COUNT(e.employee_id) AS employee_count
FROM Departments d
LEFT JOIN Employees e
	ON d.department_id = e.department_id
GROUP BY d.department_id, d.department_name;

-- Unassigned Employees
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    d.department_name,
    e.hire_date
FROM Employees e
LEFT JOIN EmployeeProjects ep
	ON e.employee_id = ep.employee_id
LEFT JOIN Departments d
	ON e.department_id = d.department_id
WHERE ep.employee_id IS NULL;

-- Department Salary Summary
SELECT
	d.department_name,
    SUM(s.salary_amount) AS total_salary,
    AVG(s.salary_amount) AS average_salary,
    COUNT(DISTINCT e.employee_id) AS employee_count
FROM Departments d
LEFT JOIN Employees e
	ON d.department_id = e.department_id
LEFT JOIN Salaries s
	ON e.employee_id = s.employee_id
GROUP BY d.department_id, d.department_name
ORDER BY total_salary DESC;

-- Employee Manager Report
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name
FROM Employees e
LEFT JOIN Employees m
	ON e.manager_id = m.employee_id;

-- Projects With More Than Three Employees
SELECT
	p.project_name,
    COUNT(ep.employee_id) AS employee_count,
    SUM(ep.hours_logged) AS total_hours
FROM Projects p
JOIN EmployeeProjects ep
	ON p.project_id = ep.project_id
GROUP BY p.project_id, p.project_name
HAVING COUNT(ep.employee_id) > 3
ORDER BY total_hours DESC;

-- Department Project Matrix
SELECT
	d.department_name,
    p.project_name,
    COUNT(ep.employee_id) AS employee_count
FROM Departments d
CROSS JOIN Projects p
LEFT JOIN Employees e
	ON e.department_id = d.department_id
LEFT JOIN EmployeeProjects ep
	ON ep.employee_id = e.employee_id
    AND ep.project_id = p.project_id
GROUP BY
	d.department_name,
    p.project_name
ORDER BY
	d.department_name,
    p.project_name;
