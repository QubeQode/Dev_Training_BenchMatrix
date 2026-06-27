using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface ITicketService
{
    Task<ParkingTicket> CreateTicketAsync(
        Vehicle vehicle,
        ParkingSpot parkingSpot,
        FeeStrategyType feeStrategyType
    );

    Task<ParkingTicket> CloseTicketAsync(
        ParkingTicket parkingTicket,
        decimal totalFee
    );
}
