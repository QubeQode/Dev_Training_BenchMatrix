using System.ComponentModel.DataAnnotations;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.DTOs;

public enum VehicleType
{
    Motorcycle,
    Car,
    Truck
}

public class ParkVehicleRequestDTO
{
    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public VehicleType VehicleType { get; set; }

    [Required]
    public FeeStrategyType FeeStrategyType { get; set; }
}
