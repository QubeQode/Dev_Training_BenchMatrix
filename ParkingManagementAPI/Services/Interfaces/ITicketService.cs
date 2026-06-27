using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface ITicketService
{
    Task<ParkingTicket> CreateTicketAsync(
        Vehicle vehicle,
        ParkingSpot parkingSpot,
        FeeStrategyType feeStrategyType
    );

    void CloseTicket(
        ParkingTicket parkingTicket,
        decimal totalFee
    );
}
