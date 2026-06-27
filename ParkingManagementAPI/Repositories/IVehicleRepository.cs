using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);

    Task<Vehicle?> GetByIdAsync(int vehicleId);

    Task AddAsync(Vehicle vehicle);

    void Update(Vehicle vehicle);

    void Remove(Vehicle vehicle);
}