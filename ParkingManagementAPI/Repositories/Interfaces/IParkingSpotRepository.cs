

using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Repositories;

public interface IParkingSpotRepository
{
    Task<IEnumerable<ParkingSpot>> GetAllAsync();

    Task AddAsync(ParkingSpot parkingSpot);

    void Update(ParkingSpot parkingSpot);

    void Remove(ParkingSpot parkingSpot);
}
