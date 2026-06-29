/*
	Functions: Calculate a value
    - Remove duplication from queries
    - Reusable calculation across multiple queries
    - Convert one value/set of values into another value
    
    PROCESS: (Input --> Business Calculation --> One Output)
    CREATE FUNCTION FunctionName 
    (
		parameter_name DataType
    )
    RETURNS (DataType)
    DETERMINISTIC/NON DETERMINISTIC

    BEGIN
		-- Process here
	END
    --------------------------------------------
    CTEs: Create temporary logical datasets
    - Break large queries into named steps
    - Stage queries into incremental processes
    - Make queries easier to understand
    
    PROCESS: (Build table A --> Use A to build B --> Use B to produce result)
    WITH CTE_Name AS (...)
    SELECT
    --------------------------------------------
    Window Functions: Perform calculations across rows without collapsing
    - Preserve every row in full detail
    - Perform aggregate calculations on every worked row
    
    PROCESS:
    RANK()/SUM()/AVG()/COUNT()
    OVER(
		PARTITION BY row_name
        ORDER BY
	)
*/

-- CTE List of Managers Whose Team Avg Salary Exceeds Manager Salary
WITH manager_employees AS
(
	SELECT
		CONCAT(m.first_name, ' ', m.last_name) AS manager_name,
        CONCAT(e.first_name, ' ', e.last_name) AS employee_name
	FROM Employees e
    JOIN Employees m
		ON e.manager_id = m.employee_id
),
direct_reports_average_salary AS
(
	SELECT
        AVG(s.salary_amount) AS average_salary
	FROM Employees e
    JOIN Salaries s
		ON e.employee_id = s.employee_id
	WHERE e.manager_id IS NOT NULL
    GROUP BY e.manager_id DESC;
)

-- CTE Above Department Average Earners
WITH department_average_salaries AS
(
	SELECT
		d.department_id,
		d.department_name,
        AVG(s.salary_amount) AS department_average_salary
	FROM Departments d
    JOIN Employees e
		ON d.department_id = e.department_id
	JOIN Salaries s
		ON s.employee_id = e.employee_id
	GROUP BY d.department_id, d.department_name
)
SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    s.salary_amount AS employee_salary,
    das.department_name,
    das.department_average_salary
FROM Salaries s
JOIN Employees e
	ON s.employee_id = e.employee_id
JOIN department_average_salaries das
	ON e.department_id = das.department_id
WHERE s.salary_amount > das.department_average_salary
ORDER BY (s.salary_amount - das.department_average_salary) DESC;

-- CTE Department Average Salaries
WITH company_average_salary AS
(
	SELECT
		AVG(s.salary_amount) AS company_average_salary
	FROM Salaries s
),
department_average_salary AS
(
	SELECT
		d.department_name,
        AVG(s.salary_amount) AS department_average_salary
	FROM Departments d
    JOIN Employees e
		ON d.department_id = e.department_id
	JOIN Salaries s
		ON s.employee_id = e.employee_id
	GROUP BY d.department_name
)
SELECT
	das.department_name,
    das.department_average_salary,
    cas.company_average_salary
FROM department_average_salary das
CROSS JOIN company_average_salary cas
WHERE das.department_salary > cas.company_average_salary
ORDER BY das.department_salary DESC;

-- Department Bonus Calculation
DELIMITER $$
CREATE FUNCTION fn_calculate_bonus
(
	p_department_name VARCHAR(20),
    p_salary_amount DECIMAL(12,2)
)
RETURNS DECIMAL(12, 2)
DETERMINISTIC

BEGIN
	DECLARE v_bonus_amount DECIMAL(12, 2);
    
    CASE
		WHEN p_department_name = 'Finance'
			THEN SET v_bonus_amount = p_salary_amount * (10 / 100);
		WHEN p_department_name = 'IT'
			THEN SET v_bonus_amount = p_salary_amount * (15 / 100);
		WHEN p_department_name = 'HR'
			THEN SET v_bonus_amount = p_salary_amount * (8 / 100);
		WHEN p_department_name = 'Marketing'
			THEN SET v_bonus_amount = p_salary_amount * (12 / 100);
		ELSE SET v_bonus_amount  = 0;
	END CASE;
    
    RETURN v_bonus_amount;
END $$
DELIMITER ;

-- Salary < 4000 = Junior, 4000-6999 = Mid-level, >=7000 = Senior
DELIMITER $$
CREATE FUNCTION fn_classify_salary
(
	p_salary_amount DECIMAL(12, 2)
)
RETURNS VARCHAR(20)
DETERMINISTIC

BEGIN
	DECLARE v_salary_classification VARCHAR(20);
    CASE
		WHEN p_salary_amount <= 4000
			THEN SET v_salary_classification = 'Junior';
		WHEN p_salary_amount > 4000 AND p_salary_amount <= 6999
			THEN SET v_salary_classification = 'Mid-level';
		WHEN p_salary_amount >= 7000
			THEN SET v_salary_classification = 'Senior';
	END CASE;
    RETURN v_salary_classification;
END $$
DELIMITER ;

-- Salary = monthly_salary * 12, create function that takes monthly salary and returns annual salary
CREATE FUNCTION fn_convert_monthly_salary_to_annual
(
	p_salary_amount DECIMAL(12, 2)
)
RETURNS DECIMAL(12, 2)
DETERMINISTIC
RETURN p_salary_amount * 12;

SELECT
	CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    s.salary_amount AS monthly_salary,
    fn_convert_monthly_salary_to_annual(s.salary_amount) AS annual_salary
FROM Employees e
JOIN Salaries s
	ON e.employee_id = s.employee_id
JOIN SalaryTypes st
	ON s.salary_type_id = st.salary_type_id
WHERE st.salary_type_name = 'Monthly';
