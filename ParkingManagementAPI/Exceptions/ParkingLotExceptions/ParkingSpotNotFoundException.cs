namespace ParkingManagementAPI.Exceptions;

public class ParkingSpotNotFoundException : ParkingLotException
{
    public ParkingSpotNotFoundException(string parkingSpotName) 
        : base($"Parking Spot {parkingSpotName} was not found."){}
}
