namespace ParkingManagementAPI.Models;

public enum FeeStrategyType
{
    Hourly,
    Daily,
    Flat
}

public class ParkingTicket
{
    public int ParkingTicketId { get; set; }

    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; } = null!;

    public int ParkingSpotId { get; set; }

    public ParkingSpot ParkingSpot { get; set; } = null!;

    public DateTime TimeOfIssuance { get; set; }

    public DateTime? TimeOfConclusion { get; set; }

    public decimal? TotalFee { get; set; }

    public FeeStrategyType FeeStrategyType { get; set; }

    public bool IsOpen => TimeOfConclusion == null;
}
