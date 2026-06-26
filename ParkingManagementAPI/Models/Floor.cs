namespace ParkingManagementAPI.Models;

public class Floor
{
    public int FloorId { get; set; }

    public int FloorNumber { get; set; }

    public int ParkingLotId { get; set; }

    public ParkingLot ParkingLot { get; set; } = null!;

    public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
}
