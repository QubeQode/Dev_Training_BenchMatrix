namespace ParkingManagementAPI.Exceptions;

public class ParkingOccupiedException : ParkingLotException
{
    public ParkingOccupiedException(string spotName) : base($"Parking spot {spotName} is already occupied."){}
}
