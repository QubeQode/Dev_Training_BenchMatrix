namespace ParkingManagementAPI.Exceptions;

public class ParkingLotNotFoundException : ParkingLotException
{
    public ParkingLotNotFoundException(string parkingLotName) 
        : base($"Parking Lot {parkingLotName} was not found."){}
}
