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

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    [Required]
    public bool PassengerIsHandicapped { get; set; } = false;

    [Required]
    public VehicleType VehicleType { get; set; }

    [Required]
    public FeeStrategyType FeeStrategyType { get; set; }
}
