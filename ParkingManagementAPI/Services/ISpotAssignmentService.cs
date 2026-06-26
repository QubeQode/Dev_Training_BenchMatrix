/*
    Find Parking Spot:
    - FindAvailableSpotAsync() - READ ONLY [Still include tracking since modification intended right after]
        - Search every floor
        - Search every spot 
        - Use ParkingSpot.AcceptableVehicle(vehicle)
        - Return first valid spot entity
        - Else return NoAvailableParking
*/
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface ISpotAssignmentService
{
    Task<ParkingSpot> FindAvailableSpotAsync(Vehicle vehicle);
}
