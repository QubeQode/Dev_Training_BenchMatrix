using ParkingManagementAPI.Exceptions;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public class TicketService : ITicketService
{
    public async Task<ParkingTicket> CreateTicketAsync(
        Vehicle vehicle,
        ParkingSpot parkingSpot,
        FeeStrategyType feeStrategyType
    )
    {
        var ticket = new ParkingTicket
        {
            Vehicle = vehicle,
            ParkingSpot = parkingSpot,
            TimeOfIssuance = DateTime.UtcNow,
            FeeStrategyType = feeStrategyType
        };

        return await Task.FromResult(ticket);
    }

    public Task CloseTicket(
        ParkingTicket parkingTicket,
        decimal totalFee
    )
    {
        if (!parkingTicket.IsOpen)
        {
            throw new TicketAlreadyClosedException(parkingTicket.ParkingTicketId);
        }

        parkingTicket.TotalFee = totalFee;

        return Task.CompletedTask;
    }
}
