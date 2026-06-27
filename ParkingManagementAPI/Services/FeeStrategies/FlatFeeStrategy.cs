using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public class FlatFeeStrategy: IFeeStrategy
{
    private const decimal FlatRate = 200;

    public FeeStrategyType StrategyType => FeeStrategyType.Flat;

    public decimal CalculateFee(TimeSpan duration)
    {
        return FlatRate;
    }
}
