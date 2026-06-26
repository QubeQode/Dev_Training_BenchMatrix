/*
    Orchestrator Service:
    - Handle Parking Event
        - Receive parking requests
        - Call SpotAssignmentService for free spot
        - Call TicketService for ticket creation
        - Occupy spot -
        - Save changes - SaveChangesAsync()
        - Return ticket DTO
    - Handle Vehicle Exit
        - Load ticket
        - Calculate fee with ParkingFeeService
        - Call TicketService for closing ticket
        - Free parking spot
        - Save changes - SaveChangesAsync()
        - Return closedTicket DTO
    - Get Active Tickets - READ ONLY
        - Search tickets WHERE exit time is NULL
        - Return list of said tickets as DTOs
    - Get Ticket by ID - READ ONLY
        - Search tickets WHERE ID provided = ticket ID
        - Return ticket DTO
    - Get Vehicle History - READ ONLY
        - Search tickets WHERE vehicle license = ticket vehicle license
        - Return list of said tickets as DTOs
*/