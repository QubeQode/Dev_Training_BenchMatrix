namespace ParkingManagementAPI.Models;

public class HandicappedSpot : ParkingSpot
{
    public override bool AcceptableVehicle(Vehicle vehicle)
    {
        return (vehicle is Motorcycle || vehicle is Car) && vehicle.PassengerIsHandicapped;
    }
}
