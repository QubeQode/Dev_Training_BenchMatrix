using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public class HourlyFeeStrategy: IFeeStrategy
{
    private const decimal HourlyRate = 50;

    public FeeStrategyType StrategyType => FeeStrategyType.Hourly;
    
    public decimal CalculateFee(TimeSpan duration)
    {
        var hours = (decimal)Math.Ceiling(duration.TotalHours);

        return hours * HourlyRate;
    }
}
