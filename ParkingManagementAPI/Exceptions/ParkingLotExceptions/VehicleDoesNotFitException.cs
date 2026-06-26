namespace ParkingManagementAPI.Exceptions;

public class VehicleDoesNotFitException : ParkingLotException
{
    public VehicleDoesNotFitException (string licensePlate, string parkingSpotName) 
        : base($"Vehicle with license plate {licensePlate} doesn't fit in parking spot {parkingSpotName}."){}
}
