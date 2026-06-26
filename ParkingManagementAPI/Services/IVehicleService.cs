/*
    Find existing vehicle by license plate or create new vehicle:
    - Find existing vehicle by license plate
    - Create vehicle if doesn't exist
    - Update vehicle information if necessary
    - Return valid vehicle entity
*/

using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface IVehicleService
{
    Task<Vehicle> ResolveVehicleAsync(ParkVehicleRequestDTO dto);
}
