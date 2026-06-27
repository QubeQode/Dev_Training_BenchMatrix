using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Mappers;
using ParkingManagementAPI.Models;
using ParkingManagementAPI.Repositories;

namespace ParkingManagementAPI.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle> ResolveVehicleAsync(ParkVehicleRequestDTO dto)
    {
        var vehicle = await _vehicleRepository.GetByLicensePlateAsync(dto.LicensePlate);

        if (vehicle is null)
        {
            vehicle = ParkingMapper.ToVehicle(dto);

            await _vehicleRepository.AddAsync(vehicle);

            return vehicle;
        }

        bool vehicleChanged = false;

        if (vehicle.Make != dto.Make)
        {
            vehicle.Make = dto.Make;
            vehicleChanged = true;
        }

        if (vehicle.Model != dto.Model)
        {
            vehicle.Model = dto.Model;
            vehicleChanged = true;
        }

        if (vehicle.PassengerIsHandicapped != dto.PassengerIsHandicapped)
        {
            vehicle.PassengerIsHandicapped = dto.PassengerIsHandicapped;
            vehicleChanged = true;
        }

        if (vehicleChanged)
        {
            _vehicleRepository.Update(vehicle);
        }

        return vehicle;
    }
}
