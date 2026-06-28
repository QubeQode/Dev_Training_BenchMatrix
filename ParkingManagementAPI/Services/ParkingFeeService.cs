using ParkingManagementAPI.Exceptions;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public class ParkingFeeService : IParkingFeeService
{
    private readonly IEnumerable<IFeeStrategy> _feeStrategies;

    public ParkingFeeService(IEnumerable<IFeeStrategy> feeStrategies)
    {
        _feeStrategies = feeStrategies;
    }

    public Task<decimal> CalculateFeeAsync(ParkingTicket ticket)
    {
        DateTime conclusionTime = 
            ticket.TimeOfConclusion 
            ?? throw new TicketStillOpenException(ticket.ParkingTicketId);

        var duration = conclusionTime - ticket.TimeOfIssuance;

        var strategy = _feeStrategies.First(s => s.StrategyType == ticket.FeeStrategyType);

        if (strategy is null)
        {
            throw new InvalidFeeStrategyTypeException(ticket.FeeStrategyType);
        }

        decimal fee = strategy.CalculateFee(duration);

        return Task.FromResult(fee);
    }
}
