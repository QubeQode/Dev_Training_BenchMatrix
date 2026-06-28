using Microsoft.EntityFrameworkCore;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Data;

public class ParkingLotDbContext : DbContext
{
    public ParkingLotDbContext
        (DbContextOptions<ParkingLotDbContext> options): base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    public DbSet<ParkingTicket> ParkingTickets => Set<ParkingTicket>();

    public DbSet<ParkingSpot> ParkingSpots => Set<ParkingSpot>();

    public DbSet<Floor> Floors => Set<Floor>();

    public DbSet<ParkingLot> ParkingLots => Set<ParkingLot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingLot>()
            .HasMany(pl => pl.Floors)
            .WithOne(f => f.ParkingLot)
            .HasForeignKey(f => f.ParkingLotId);

        modelBuilder.Entity<Floor>()
            .HasMany(f => f.ParkingSpots)
            .WithOne(ps => ps.Floor)
            .HasForeignKey(ps => ps.FloorId);
        
        modelBuilder.Entity<Vehicle>()
            .HasMany(v => v.TicketHistory)
            .WithOne(t => t.Vehicle)
            .HasForeignKey(t => t.VehicleId);
        
        modelBuilder.Entity<ParkingSpot>()
            .HasMany(ps => ps.TicketHistory)
            .WithOne(t => t.ParkingSpot)
            .HasForeignKey(t => t.ParkingSpotId);
        
        modelBuilder.Entity<ParkingSpot>()
            .HasIndex(ps => ps.ParkingSpotName)
            .IsUnique();
        
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.LicensePlate)
            .IsUnique();
        
        modelBuilder.Entity<ParkingLot>().HasData(
            new ParkingLot
            {
                ParkingLotId = 1,
                ParkingLotName = "Atreus Downtown Parking",
                Address = "1501 Main Street"
            }  
        );

        modelBuilder.Entity<Floor>().HasData(
            new Floor
            {
                FloorId = 1,
                FloorNumber = 1,
                ParkingLotId = 1
            },
            new Floor
            {
                FloorId = 2,
                FloorNumber = 2,
                ParkingLotId = 1
            }
        );

        modelBuilder.Entity<Car>().HasData(
            new Car
            {
                VehicleId = 1,
                LicensePlate = "ZIX 2395",
                Make = "Toyota",
                Model = "Corolla",
                PassengerIsHandicapped = false
            },
            new Car
            {
                VehicleId = 2,
                LicensePlate = "SWF 0098",
                Make = "Honda",
                Model = "Civic",
                PassengerIsHandicapped = false
            },
            new Car
            {
                VehicleId = 3,
                LicensePlate = "GKX 3702",
                Make = "Hyundai",
                Model = "Elantra",
                PassengerIsHandicapped = true
            }
        );

        modelBuilder.Entity<Motorcycle>().HasData(
            new Motorcycle
            {
                VehicleId = 4,
                LicensePlate = "ZTZ 8530",
                Make = "Yamaha",
                Model = "Mt-07",
                PassengerIsHandicapped = false
            },
            new Motorcycle
            {
                VehicleId = 5,
                LicensePlate = "KVM 0203",
                Make = "Kawasaki",
                Model = "Ninja 650",
                PassengerIsHandicapped = false
            }
        );

        modelBuilder.Entity<Truck>().HasData(
            new Truck
            {
                VehicleId = 6,
                LicensePlate = "SPV 5902",
                Make = "Freightliner",
                Model = "Cascadia",
                PassengerIsHandicapped = false
            },
            new Truck
            {
                VehicleId = 7,
                LicensePlate = "NNJ 2512",
                Make = "Kenworth",
                Model = "T680",
                PassengerIsHandicapped = false
            },
            new Truck
            {
                VehicleId = 8,
                LicensePlate = "ZNO 1823",
                Make = "Peterbilt",
                Model = "579",
                PassengerIsHandicapped = false
            }
        );

        // Using anonymous objects because of private setters
        modelBuilder.Entity<CompactSpot>().HasData( 
            new
            {
                ParkingSpotId = 1,
                ParkingSpotName = "A01",
                FloorId = 1,
                IsOccupied = false
            },
            new
            {
                ParkingSpotId = 2,
                ParkingSpotName = "A02",
                FloorId = 1,
                IsOccupied = false
            },
            new
            {
                ParkingSpotId = 5,
                ParkingSpotName = "B01",
                FloorId = 2,
                IsOccupied = false
            },
            new
            {
                ParkingSpotId = 6,
                ParkingSpotName = "B02",
                FloorId = 2,
                IsOccupied = false
            }  
        );

        modelBuilder.Entity<HandicappedSpot>().HasData(
            new
            {
                ParkingSpotId = 3,
                ParkingSpotName = "A03",
                FloorId = 1,
                IsOccupied = false
            },
            new
            {
                ParkingSpotId = 7,
                ParkingSpotName = "B03",
                FloorId = 2,
                IsOccupied = false
            }
        );

        modelBuilder.Entity<LargeSpot>().HasData(
            new
            {
                ParkingSpotId = 4,
                ParkingSpotName = "A04",
                FloorId = 1,
                IsOccupied = false
            },
            new
            {
                ParkingSpotId = 8,
                ParkingSpotName = "B04",
                FloorId = 2,
                IsOccupied = false
            }
        );
    }
}
