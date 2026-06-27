using Microsoft.EntityFrameworkCore;
using ParkingManagementAPI.Data;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ParkingLotDbContext _context;

    public VehicleRepository(ParkingLotDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public void Update(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
    }

    public void Remove(Vehicle vehicle)
    {
        _context.Vehicles.Remove(vehicle);
    }
}
