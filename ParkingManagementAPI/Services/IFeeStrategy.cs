namespace ParkingManagementAPI.Services;

public interface IFeeStrategy
{
    decimal CalculateFee(TimeSpan duration);
}
