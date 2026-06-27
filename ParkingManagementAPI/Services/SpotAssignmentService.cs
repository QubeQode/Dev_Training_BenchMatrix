using ParkingManagementAPI.Exceptions;
using ParkingManagementAPI.Models;
using ParkingManagementAPI.Repositories;

namespace ParkingManagementAPI.Services;

public class SpotAssignmentService : ISpotAssignmentService
{
    private readonly IParkingSpotRepository _parkingSpotRepository;

    public SpotAssignmentService(IParkingSpotRepository parkingSpotRepository)
    {
        _parkingSpotRepository = parkingSpotRepository;
    }

    public async Task<ParkingSpot> FindAvailableSpotAsync(Vehicle vehicle)
    {
        var availableSpots = await _parkingSpotRepository.GetAllAsync();

        bool compatibleSpotExists = availableSpots.Any(s => s.AcceptableVehicle(vehicle));

        if (!compatibleSpotExists)
        {
            throw new VehicleDoesNotFitException(vehicle.LicensePlate);
        }
        
        var parkableSpot = availableSpots
            .Where(s => !s.IsOccupied)
            .OrderBy(s => s.Floor.FloorNumber)
            .ThenBy(s => s.ParkingSpotName)
            .FirstOrDefault(s => s.AcceptableVehicle(vehicle));
        
        if (parkableSpot is null)
        {
            throw new NoAvailableParkingException();
        }

        return parkableSpot;

    }
}
