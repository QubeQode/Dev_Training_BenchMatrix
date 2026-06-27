using Microsoft.EntityFrameworkCore;
using ParkingManagementAPI.Data;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly ParkingLotDbContext _context;

    TicketRepository(ParkingLotDbContext context)
    {
        _context = context;
    }

    public async Task<ParkingTicket?> GetByIdAsync (int ticketId)
    {
        return await _context.ParkingTickets
            .AsNoTracking()
            .Include(t => t.Vehicle)
            .Include(t => t.ParkingSpot)
                .ThenInclude(s => s.Floor)
            .FirstOrDefaultAsync(t => t.ParkingTicketId == ticketId);
        
    }

    public async Task<ParkingTicket?> GetActiveTicketByIdAsync(int ticketId)
    {
        return await _context.ParkingTickets
            .Include(t => t.ParkingSpot)
            .FirstOrDefaultAsync(t =>
                t.ParkingTicketId == ticketId &&
                t.TimeOfConclusion == null
            );
    }

    public async Task<IEnumerable<ParkingTicket>> GetActiveTicketsAsync()
    {
        return await _context.ParkingTickets
            .AsNoTracking()
            .Include(t => t.Vehicle)
            .Include(t => t.ParkingSpot)
                .ThenInclude(s => s.Floor)
            .Where(t => t.TimeOfConclusion == null)
            .ToListAsync();
    }

    public async Task<bool> HasOpenTicketAsync(int vehicleId)
    {
        return await _context.ParkingTickets
            .AnyAsync(t =>
                t.VehicleId == vehicleId &&
                t.TimeOfConclusion == null
            );
    }

    public async Task<IEnumerable<ParkingTicket>> GetVehicleHistoryAsync(string licensePlate)
    {
        return await _context.ParkingTickets
            .AsNoTracking()
            .Include(t => t.Vehicle)
            .Include(t => t.ParkingSpot)
                .ThenInclude(s => s.Floor)
            .Where(t => t.Vehicle.LicensePlate == licensePlate)
            .OrderByDescending(t => t.TimeOfIssuance)
            .ToListAsync();
    }

    public async Task AddAsync(ParkingTicket ticket)
    {
        await _context.ParkingTickets.AddAsync(ticket);
    }

    public void Update(ParkingTicket ticket)
    {
        _context.ParkingTickets.Update(ticket);
    }

    public void Remove(ParkingTicket ticket)
    {
        _context.ParkingTickets.Remove(ticket);
    }
}
