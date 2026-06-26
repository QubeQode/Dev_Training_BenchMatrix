using Microsoft.AspNetCore.SignalR;

namespace ParkingManagementAPI.Models;

public abstract class Vehicle
{
    public int VehicleId { get; set; }

    public string LicensePlate { get; private set; } = string.Empty;

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public bool PassengerIsHandicapped { get; set; } = false;

    public ICollection<ParkingTicket> TicketHistory { get; set; } = new List<ParkingTicket>();
}
