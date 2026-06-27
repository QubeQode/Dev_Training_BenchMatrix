using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Repositories;

public interface ITicketRepository
{
    Task<ParkingTicket?> GetByIdAsync(int ticketId);

    Task<ParkingTicket?> GetActiveTicketByIdAsync(int ticketId);

    Task<IEnumerable<ParkingTicket>> GetActiveTicketsAsync();

    Task<IEnumerable<ParkingTicket>> GetVehicleHistoryAsync(string licensePlate);

    Task AddAsync(ParkingTicket ticket);

    void Update(ParkingTicket ticket);

    void Remove(ParkingTicket ticket);
}