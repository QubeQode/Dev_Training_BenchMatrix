using ParkingManagementAPI.Exceptions;

namespace ParkingManagementAPI.Models;

public abstract class ParkingSpot
{
    public int ParkingSpotId { get; set; }

    public string ParkingSpotName { get; set; } = string.Empty;

    public int FloorId { get; set; }

    public Floor Floor { get; set; } = null!;

    public bool IsOccupied { get; private set; }

    public ICollection<ParkingTicket> TicketHistory { get; set; } = new List<ParkingTicket>();

    public void OccupySpot()
    {
        if (IsOccupied) throw new ParkingOccupiedException(ParkingSpotName);

        IsOccupied = true;
    }

    public void FreeSpot()
    {
        if (!IsOccupied) throw new ParkingFreeException(ParkingSpotName);

        IsOccupied = false;
    }

    public abstract bool AcceptableVehicle(Vehicle vehicle);
}
