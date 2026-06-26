using Microsoft.VisualBasic;

namespace ParkingManagementAPI.Models;

public class ParkingLot
{
    public int ParkingLotId { get; set; }

    public string ParkingLotName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public ICollection<Floor> Floors { get; set; } = new List<Floor>();
}
