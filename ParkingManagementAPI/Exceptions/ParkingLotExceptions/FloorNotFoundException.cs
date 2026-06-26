namespace ParkingManagementAPI.Exceptions;

public class FloorNotFoundException : ParkingLotException
{
    public FloorNotFoundException(int floorNumber) 
        : base($"Floor {floorNumber} was not found."){}
}
