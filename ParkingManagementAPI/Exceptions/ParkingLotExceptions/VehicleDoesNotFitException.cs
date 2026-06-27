namespace ParkingManagementAPI.Exceptions;

public class VehicleDoesNotFitException : ParkingLotException
{
    public VehicleDoesNotFitException (string licensePlate) 
        : base($"Vehicle with license plate {licensePlate} doesn't fit in available parking spots."){}
}
