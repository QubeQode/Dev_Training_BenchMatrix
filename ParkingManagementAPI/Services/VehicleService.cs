using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Exceptions;
using ParkingManagementAPI.Mappers;
using ParkingManagementAPI.Models;
using ParkingManagementAPI.Repositories;

namespace ParkingManagementAPI.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITicketRepository _ticketRepository;

    public VehicleService(
        IVehicleRepository vehicleRepository,
        ITicketRepository ticketRepository
    )
    {
        _vehicleRepository = vehicleRepository;
        _ticketRepository = ticketRepository;
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

        bool hasOpenTicket = await _ticketRepository.HasOpenTicketAsync(vehicle.VehicleId);

        if (hasOpenTicket)
        {
            throw new VehicleAlreadyParkedException(vehicle.LicensePlate);
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
