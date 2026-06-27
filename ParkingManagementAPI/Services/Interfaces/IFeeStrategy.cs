using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface IFeeStrategy
{
    FeeStrategyType StrategyType { get; }
    
    decimal CalculateFee(TimeSpan duration);
}
