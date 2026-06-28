using ParkingManagementAPI.DTOs;

namespace ParkingManagementAPI.Exceptions;

public class InvalidVehicleTypeException : ParkingLotException
{
    public InvalidVehicleTypeException (VehicleType vehicleType) 
        : base($"The input vehicle type {vehicleType} does not exist."){}
}