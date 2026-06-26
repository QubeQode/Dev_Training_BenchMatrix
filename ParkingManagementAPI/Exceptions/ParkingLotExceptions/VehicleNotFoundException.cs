namespace ParkingManagementAPI.Exceptions;

public class VehicleNotFoundException : ParkingLotException
{
    public VehicleNotFoundException(string licensePlate) 
        : base($"Vehicle with license plate {licensePlate} was not found."){}
}
