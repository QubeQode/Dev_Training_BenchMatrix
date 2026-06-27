using Microsoft.EntityFrameworkCore;
using ParkingManagementAPI.Data;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Repositories;

public class ParkingSpotRepository : IParkingSpotRepository
{
    private readonly ParkingLotDbContext _context;

    public ParkingSpotRepository(ParkingLotDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParkingSpot>> GetAllAsync()
    {
        return await _context.ParkingSpots
            .Include(s => s.Floor)
            .ToListAsync();
    }

    public async Task AddAsync(ParkingSpot parkingSpot)
    {
        await _context.ParkingSpots.AddAsync(parkingSpot);
    }

    public void Update(ParkingSpot parkingSpot)
    {
        _context.ParkingSpots.Update(parkingSpot);
    }

    public void Remove(ParkingSpot parkingSpot)
    {
        _context.ParkingSpots.Remove(parkingSpot);
    }
}
