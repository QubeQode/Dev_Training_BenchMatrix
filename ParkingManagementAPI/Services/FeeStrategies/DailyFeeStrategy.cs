using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public class DailyFeeStrategy: IFeeStrategy
{
    private const decimal DailyRate = 500;

    public FeeStrategyType StrategyType => FeeStrategyType.Daily;

    public decimal CalculateFee(TimeSpan duration)
    {
        var days = (decimal)Math.Ceiling(duration.TotalDays);

        return days * DailyRate;
    }
}
