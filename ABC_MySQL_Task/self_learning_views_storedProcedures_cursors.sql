/*
	Views: Create reusable business-facing datasets
    - Produce reusable JOIN for multiple queries
    - Present a complex query as a table
    
    PROCESS:
    CREATE VIEW ViewName
    AS
    SELECT
    --------------------------------------------
    Stored Procedures: Perform a business process
    - Perform a workflow in a staged sequence
    - Chain multiple database actions together
    
    PROCESS:
    CREATE PROCEDURE ProcedureName
    (
		IN input_parameter DataType
        OUT output_parameter DataType
    )
    BEGIN
		-- Process here
	END
    --------------------------------------------
    Cursors: Process rows one at a time
    - Resolve some calculation that can't be solved in a set based manner
    
    PROCESS:
    OPEN cursor_name
    read_loop: loop
		FETCH cursor_name INTO v_variable_name
        -- Process here
	END LOOP
    CLOSE cursor_name
    --------------------------------------------
    Indexes: Make data retrieval faster
    - Avoid scanning a table and instead jump directly to the row desired
    - Performance enhancement and not a logic solution
    
    PROCESS:
    CREATE INDEX idx_name ON table_name (column_name1 [, columnName2...]);
*/