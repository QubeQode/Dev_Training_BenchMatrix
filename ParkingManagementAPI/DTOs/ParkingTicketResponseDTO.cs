using System.ComponentModel.DataAnnotations;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.DTOs;

public class ParkingTicketResponseDTO
{
    public int TicketId { get; init; }

    public int FloorNumber { get; init; }

    public string ParkingSpotName { get; init; } = string.Empty;

    public DateTime TimeOfIssuance { get; init; }

    public DateTime? TimeOfConclusion { get; init; }

    public decimal? TotalFee { get; init; }

    public FeeStrategyType FeeStrategyType { get; init; }

    public bool IsOpen { get; init; }
}
