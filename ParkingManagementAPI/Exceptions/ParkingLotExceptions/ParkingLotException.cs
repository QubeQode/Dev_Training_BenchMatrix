namespace ParkingManagementAPI.Exceptions;

public abstract class ParkingLotException : Exception
{
    protected ParkingLotException(string message) : base(message) {}
}
