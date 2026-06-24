USE abc_company;
/*
	Example: Give me a list of employees whose department doesn't match that of their managers
    Step 1: Identify the entities involved 
    - Employee, Department, Manager, Department
    
    Step 2: Identify the relationship involved
    - Employee table needs to self join with itself for Employee-Manager
    - Employees e JOIN Employees m ON e.manager_id = m.employee_id
    - A.k.a where the manager ID on an employee matches the employee ID on a manager
    - This is "all manager-employee relationships"
    
    Step 3: Determine what makes a row qualify - The "Filter"
    - Department doesn't match manager department
    - WHERE e.department_id <> m.department_id
    - A.k.a where the employee department ID != manager department ID
    
    Step 4: Determine what needs to be displayed
    - Employee Name, Employee Department, Manager Name, Manager Department
    
    Step 5: Establish further JOINs needed to display all information
    - JOIN Departments ed ON e.department_id = ed.department_id
    - JOIN Departments md ON m.department_id = md.department_id
    
    Step 6: Build the Query
    
    CHEATSHEET:
    - FROM = Starting Table
    - JOIN = Define Relationship
    - WHERE = Filtering Single Rows
    - GROUP BY = Summarizing Multiple Rows
    - HAVING = Filtering Summarized Rows
    - ORDER BY = Sorting Output
*/
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    ed.department_name AS employee_department,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    md.department_name AS manager_department
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id
JOIN Departments ed
	ON e.department_id = ed.department_id
JOIN Departments md
	ON m.department_id = md.department_id
WHERE e.department_id <> m.department_id;

-- Produce a report showing every project where employees from more than one department are assigned
-- Sort by number of departments descending

-- Produce a list of departments with no employee who is a manager to another employee


-- Produce a list of employees whose manager belongs to diff department, sort by employee dept
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    ed.department_name AS employee_department,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    md.department_name AS manager_department
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id
JOIN Departments ed
	ON e.department_id = ed.department_id
JOIN Departments md
	ON m.department_id = md.department_id
WHERE e.department_id <> m.department_id
ORDER BY ed.department_name ASC;

-- Produce a list of all employees assigned to at least one project their manager is also assigned to
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    p.project_name
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id
JOIN EmployeeProjects ep
	ON e.employee_id = ep.employee_id
JOIN EmployeeProjects mp
	ON m.employee_id = mp.employee_id
    AND ep.project_id = mp.project_id -- Collapse duplicates into singles
JOIN Projects p
	ON ep.project_id = p.project_id
ORDER BY p.project_name, employee_name DESC;

-- Produce a list of all managers and the number of employees directly reporting to them, descending
SELECT
	CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    COUNT(e.employee_id) AS subordinate_number
FROM Employees m
JOIN Employees e
	ON m.employee_id = e.manager_id
GROUP BY m.employee_id
ORDER BY subordinate_number DESC;

-- Produce a list of departments and the number of distinct employees from each department
-- who are assigned to projects
SELECT
	d.department_name,
    COUNT(e.employee_id) AS assigned_employees
FROM Departments d
JOIN Employees e
	ON d.department_id = e.department_id
JOIN EmployeeProjects ep
	ON e.employee_id = ep.employee_id
GROUP BY d.department_name
ORDER BY assigned_employees DESC;

-- Produce a list of employees whose salary is greater than their manager's salary
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    CASE
		WHEN es.salary_type_id = 1
			THEN es.salary_amount * 12
		WHEN es.salary_type_id = 2
			THEN es.salary_amount
	END AS employee_salary,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    CASE
		WHEN ms.salary_type_id = 1
			THEN ms.salary_amount * 12
		WHEN ms.salary_type_id = 2
			THEN ms.salary_amount
	END AS manager_salary
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id
JOIN Salaries es
	ON e.employee_id = es.employee_id
JOIN Salaries ms
	ON m.employee_id = ms.employee_id
WHERE
	CASE
		WHEN es.salary_type_id = 1
			THEN es.salary_amount * 12
		ELSE es.salary_amount
	END
	>
    CASE
		WHEN ms.salary_type_id = 1
			THEN ms.salary_amount * 12
        ELSE ms.salary_amount
	END
ORDER BY (es.salary_amount - ms.salary_amount) DESC;

-- Produce a report showing total salary expense of each department, sort from highest to lowest
SELECT
	d.department_name,
    SUM(
		CASE
			WHEN s.salary_type_id = 1
				THEN s.salary_amount * 12
			WHEN s.salary_type_id = 2
				THEN s.salary_amount
		END
    ) AS total_salary_expense
FROM Departments d
JOIN Employees e
	ON d.department_id = e.department_id
JOIN Salaries s
	ON e.employee_id = s.employee_id
GROUP BY d.department_name
ORDER BY total_salary_expense DESC;

-- Produce a list of employees not assigned to any projects, sort by last name
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    d.department_name
FROM Employees e
JOIN Departments d
	ON e.department_id = d.department_id
LEFT JOIN EmployeeProjects ep
	ON e.employee_id = ep.employee_id
WHERE ep.employee_id IS NULL
ORDER BY e.last_name DESC;

-- Produce a list of all projects along with no. of employees assigned to each project
SELECT
	p.project_name,
    COUNT(ep.employee_id) AS assigned_employees
FROM Projects p
JOIN EmployeeProjects ep
	ON p.project_id = ep.project_id
GROUP BY p.project_name
ORDER BY assigned_employees DESC;

-- Produce a list showing every employee and the projects they're assigned to, ordered by proj name + emp name
SELECT
    CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    p.project_name
FROM Employees e
JOIN EmployeeProjects ep
	ON e.employee_id = ep.employee_id
JOIN Projects p
	ON ep.project_id = p.project_id
ORDER BY p.project_name, employee_name ASC;

-- Produce a list of departments than contain more than one employee, ordered descending
SELECT
	d.department_name,
    COUNT(e.employee_id) AS employee_count
FROM Departments d
JOIN Employees e
	ON d.department_id = e.department_id
GROUP BY d.department_name
HAVING COUNT(e.employee_id) > 1
ORDER BY employee_count DESC;

-- Produce the average salary by department, ordered descending
SELECT
	d.department_name,
    AVG(
		CASE
			WHEN s.salary_type_id = 1
				THEN s.salary_amount * 12
			WHEN s.salary_type_id = 2
				THEN s.salary_amount
		END
        ) AS average_salary
FROM Departments d
JOIN Employees e
	ON  d.department_id = e.department_id
JOIN Salaries s
	ON e.employee_id = s.employee_id
GROUP BY d.department_name
ORDER BY average_salary DESC;

-- Produce a count of employees in each department, ordered desending
SELECT
	d.department_name,
    COUNT(e.employee_id) AS number_of_employees
FROM Departments d
JOIN Employees e
	ON d.department_id = e.department_id
GROUP BY d.department_id
ORDER BY number_of_employees DESC;

-- Produce a list of employees whose department differs from their manager's department
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    ed.department_name AS employee_department,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    md.department_name AS manager_department
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id
JOIN Departments ed
	ON e.department_id = ed.department_id
JOIN Departments md
	ON m.department_id = md.department_id
WHERE e.department_id <> m.department_id;

-- Produce a list of employees, their department and their managers, sorted by dep name and then last name
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
    d.department_name
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id
JOIN Departments d
	ON e.department_id = d.department_id
ORDER BY d.department_name, e.last_name ASC;

-- Produce a list of all employees and their managers, exclude all employees without a manager
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    CONCAT(m.first_name, ' ', m.last_name) AS manager_name
FROM Employees e
JOIN Employees m
	ON e.manager_id = m.employee_id; -- INNER JOIN implicitly handles the filtering

-- Produce a list of all employees in the Finance department - display alphabetically by first name:
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    d.department_name
FROM Employees e
JOIN Departments d
	ON e.department_id = d.department_id
WHERE d.department_name = 'Finance'
ORDER BY e.first_name ASC;

-- Produce a list of all employees and the departments they belong to - display alphabetically by last name:
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    d.department_name
FROM Employees e
LEFT JOIN Departments d
	ON e.department_id = d.department_id
ORDER BY e.last_name ASC;
