namespace ParkingManagementAPI.Models;

public class LargeSpot : ParkingSpot
{
    public override bool AcceptableVehicle(Vehicle vehicle)
    {
        return vehicle is Truck;
    }
}
