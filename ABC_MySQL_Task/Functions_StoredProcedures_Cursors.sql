-- Employee Tenure Function
DELIMITER $$
CREATE FUNCTION fn_get_emp_tenure
(
	p_employee_id INT
)
RETURNS INT
DETERMINISTIC

BEGIN
	DECLARE v_years INT;
    SELECT TIMESTAMPDIFF
    (
		YEAR,
        hire_date,
        CURDATE()
    )
    INTO v_years
    FROM Employees
    WHERE employee_id = p_employee_id;
    
    RETURN v_years;
END$$
DELIMITER ;

-- Test
SELECT
	employee_id,
    first_name,
    fn_get_emp_tenure(employee_id) AS tenure_years
FROM Employees;

-- Annual Salary Function
DELIMITER $$
CREATE FUNCTION fn_annual_salary
(
	p_employee_id INT
)
RETURNS DECIMAL (12,2)
DETERMINISTIC

BEGIN
	DECLARE v_salary DECIMAL (12, 2);
    DECLARE v_type VARCHAR(20);
    
    SELECT
		s.salary_amount,
        st.salary_type_name
	INTO
		v_salary,
        v_type
	FROM Salaries s
    JOIN SalaryTypes st
		ON s.salary_type_id = st.salary_type_id
	WHERE s.employee_id = p_employee_id
    ORDER BY effective_date DESC
    LIMIT 1;
    
    IF v_salary is NULL THEN
		RETURN 0;
	END IF;
    
    IF v_type = 'Monthly' THEN
		RETURN v_salary * 12;
	END IF;
    
    RETURN v_salary;
END $$
DELIMITER ;

-- Department Employees View
CREATE VIEW vw_department_employees AS
SELECT
	e.employee_id,
    e.department_id,
    CONCAT(e.first_name, ' ', e.last_name) AS employee_name,
    fn_annual_salary(e.employee_id) AS annual_salary,
    fn_get_emp_tenure(e.employee_id) AS tenure
FROM Employees e;

-- Test
SELECT *
FROM vw_department_employees;

-- Department Salary Report Procedure
DELIMITER $$
CREATE PROCEDURE sp_dept_salary_report
(
	IN p_department_id INT,
    OUT p_employee_count INT,
    OUT p_total_salary DECIMAL (12, 2),
    OUT p_avg_salary DECIMAL (12, 2),
    OUT p_top_earner VARCHAR(100)
)

BEGIN
	SELECT
		CONCAT(e.first_name, ' ', e.last_name) employee_name,
        s.salary_amount
	FROM Employees e
    LEFT JOIN Salaries s
		ON e.employee_id = s.employee_id
	WHERE e.department_id = p_department_id;
    
    SELECT COUNT(*)
    INTO p_employee_count
    FROM Employees
    WHERE department_id = p_department_id;
    
    SELECT SUM(s.salary_amount)
    INTO p_total_salary
    FROM Employees e
    JOIN Salaries s
		ON e.employee_id = s.employee_id
	WHERE e.department_id = p_department_id;
    
    SELECT AVG(s.salary_amount)
    INTO p_avg_salary
    FROM Employees e
    JOIN Salaries s
		ON e.employee_id = s.employee_id
	WHERE e.department_id = p_department_id;
    
    SELECT
		CONCAT(e.first_name, ' ', e.last_name)
	INTO p_top_earner
    FROM Employees e
    JOIN Salaries s
		ON e.employee_id = s.employee_id
	WHERE e.department_id = p_department_id
    ORDER BY s.salary_amount DESC
    LIMIT 1;
END $$
DELIMITER ;

-- Give Department Raise
DELIMITER $$
CREATE PROCEDURE sp_give_raise
(
	IN p_department_id INT,
    IN p_percentage DECIMAL (5, 2)
)

BEGIN
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
		ROLLBACK;
        
        SELECT
		'Raise failed. Transaction rolled back.'
        AS Message;
	END;
	
    START TRANSACTION;
    UPDATE Salaries s
    JOIN Employees e
		ON s.employee_id = e.employee_id
	
    SET s.salary_amount =
		s.salary_amount * (1 + p_percentage/100)
    WHERE e.department_id = p_department_id
    AND e.is_active = TRUE;
    
    COMMIT;
    
    SELECT
    'Raise completed successfully.'
    AS Message;
END $$
DELIMITER ;
