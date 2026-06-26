/*
    Ticket Creation + Ticket Closing
    - Ticket Creation
        - (Vehicle vehicle, ParkingSpot parkingSpot, FeeStrategyType feeStrategyType)
        - Assign entry time
        - Assign vehicle to ticket
        - Assign parking spot to ticket
        - Assign fee strategy type to ticket
        - Return ticket entity
    - Ticket Closing
        - (ParkingTicket ticket, decimal totalFee)
        - Assign conclusion time to ticket
        - Assign value to totalFee
        - Return closedTicket entity
*/
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface ITicketService
{
    ParkingTicket CreateTicketAsync(
        Vehicle vehicle,
        ParkingSpot parkingSpot,
        FeeStrategyType feeStrategyType
    );

    ParkingTicket CloseTicketAsync(
        ParkingTicket parkingTicket,
        decimal totalFee
    );
}
