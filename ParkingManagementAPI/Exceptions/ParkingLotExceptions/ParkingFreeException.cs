namespace ParkingManagementAPI.Exceptions;

public class ParkingFreeException : ParkingLotException
{
    public ParkingFreeException(string spotName) 
        : base($"Parking spot {spotName} is already free."){}
}
