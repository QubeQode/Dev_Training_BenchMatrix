namespace ParkingManagementAPI.Models;

public class CompactSpot : ParkingSpot
{
    public override bool AcceptableVehicle(Vehicle vehicle)
    {
        return vehicle is Motorcycle || vehicle is Car;
    }
}
