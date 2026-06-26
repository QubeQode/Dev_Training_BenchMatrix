using System.ComponentModel.DataAnnotations;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.DTOs;

public enum ParkingSpotType
{
    Compact,
    Handicapped,
    Large
}

public class ParkingTicketResponseDTO
{
    public int TicketId { get; init; }

    public string LicensePlate { get; init; } = string.Empty;

    public int FloorNumber { get; init; }

    public string ParkingSpotName { get; init; } = string.Empty;

    public ParkingSpotType SpotType { get; init; }

    public DateTime TimeOfIssuance { get; init; }

    public DateTime? TimeOfConclusion { get; init; }

    public decimal? TotalFee { get; init; }

    public FeeStrategyType FeeStrategyType { get; init; }

    public bool IsOpen { get; init; }
}
