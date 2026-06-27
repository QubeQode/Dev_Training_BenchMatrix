using ParkingManagementAPI.Data;
using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Exceptions;
using ParkingManagementAPI.Mappers;
using ParkingManagementAPI.Repositories;

namespace ParkingManagementAPI.Services;

public class ParkingService : IParkingService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IParkingSpotRepository _parkingSpotRepository;
    private readonly IVehicleService _vehicleService;
    private readonly ISpotAssignmentService _spotAssignmentService;
    private readonly ITicketService _ticketService;
    private readonly IParkingFeeService _parkingFeeService;
    private readonly ParkingLotDbContext _context;

    public ParkingService(
        IVehicleRepository vehicleRepository,
        ITicketRepository ticketRepository,
        IParkingSpotRepository parkingSpotRepository,
        IVehicleService vehicleService,
        ISpotAssignmentService spotAssignmentService,
        ITicketService ticketService,
        IParkingFeeService parkingFeeService,
        ParkingLotDbContext context
    )
    {
        _vehicleRepository = vehicleRepository;
        _ticketRepository = ticketRepository;
        _parkingSpotRepository = parkingSpotRepository;
        _vehicleService = vehicleService;
        _spotAssignmentService = spotAssignmentService;
        _ticketService = ticketService;
        _parkingFeeService = parkingFeeService;
        _context = context;
    }

    public async Task<ParkingTicketResponseDTO> HandleParkingEventAsync(ParkVehicleRequestDTO request)
    {
        var vehicle = await _vehicleService.ResolveVehicleAsync(request);
        var parkingSpot = await _spotAssignmentService.FindAvailableSpotAsync(vehicle);
        var ticket = await _ticketService.CreateTicketAsync(
            vehicle,
            parkingSpot,
            request.FeeStrategyType
        );

        parkingSpot.OccupySpot();

        await _ticketRepository.AddAsync(ticket);
        await _context.SaveChangesAsync();
        
        return ParkingMapper.ToResponse(ticket);
    }

    public async Task<ParkingTicketResponseDTO> HandleVehicleExitEventAsync(int ticketId)
    {
        var ticket = await _ticketRepository.GetActiveTicketByIdAsync(ticketId);

        if (ticket is null)
        {
            throw new TicketNotFoundException(ticketId);
        }

        ticket.TimeOfConclusion = DateTime.UtcNow;

        var fee = await _parkingFeeService.CalculateFeeAsync(ticket);

        await _ticketService.CloseTicket(ticket, fee);

        ticket.ParkingSpot.FreeSpot();

        _ticketRepository.Update(ticket);

        await _context.SaveChangesAsync();

        return ParkingMapper.ToResponse(ticket);
    }

    public async Task<IEnumerable<ParkingTicketResponseDTO>> GetActiveTicketsAsync()
    {
        var tickets = await _ticketRepository.GetActiveTicketsAsync();

        return tickets.Select(ParkingMapper.ToResponse);
    }

    public async Task<ParkingTicketResponseDTO> GetTicketByIdAsync(int ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);

        if (ticket is null)
        {
            throw new TicketNotFoundException(ticketId);
        }

        return ParkingMapper.ToResponse(ticket);
    }

    public async Task<IEnumerable<ParkingTicketResponseDTO>> GetVehicleHistoryAsync(string licensePlate)
    {
        var tickets = await _ticketRepository.GetVehicleHistoryAsync(licensePlate);

        return tickets.Select(ParkingMapper.ToResponse);
    }

}
