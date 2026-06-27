using Microsoft.EntityFrameworkCore;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Data;

public class ParkingLotDbContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<ParkingTicket> ParkingTickets { get; set; }

    public DbSet<ParkingSpot> ParkingSpots { get; set; }

    public DbSet<Floor> Floors { get; set; }

    public DbSet<ParkingLot> ParkingLots { get; set; }
}
