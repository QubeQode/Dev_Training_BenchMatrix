using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface IVehicleService
{
    Task<Vehicle> ResolveVehicleAsync(ParkVehicleRequestDTO dto);
}
