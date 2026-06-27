using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface ISpotAssignmentService
{
    Task<ParkingSpot> FindAvailableSpotAsync(Vehicle vehicle);
}
