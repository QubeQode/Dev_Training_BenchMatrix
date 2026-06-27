using ParkingManagementAPI.Exceptions;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public class ParkingFeeService
{
    private readonly IEnumerable<IFeeStrategy> _feeStrategies;

    public ParkingFeeService(IEnumerable<IFeeStrategy> feeStrategies)
    {
        _feeStrategies = feeStrategies;
    }

    public decimal CalculateFee(ParkingTicket ticket)
    {
        DateTime conclusionTime = 
            ticket.TimeOfConclusion 
            ?? throw new TicketAlreadyClosedException(ticket.ParkingTicketId);

        var duration = conclusionTime - ticket.TimeOfIssuance;

        var strategy = _feeStrategies.First(s => s.StrategyType == ticket.FeeStrategyType);

        return strategy.CalculateFee(duration);
    }
}
