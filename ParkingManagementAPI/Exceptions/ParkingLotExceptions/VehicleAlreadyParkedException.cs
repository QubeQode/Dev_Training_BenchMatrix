namespace ParkingManagementAPI.Exceptions;

public class VehicleAlreadyParkedException : ParkingLotException
{
    public VehicleAlreadyParkedException (string licensePlate) 
        : base($"Vehicle with license plate {licensePlate} is already parked."){}
}
