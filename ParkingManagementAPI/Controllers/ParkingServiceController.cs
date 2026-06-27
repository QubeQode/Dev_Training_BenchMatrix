using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Services;

namespace ParkingManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController : ControllerBase
{
    private readonly IParkingService _parkingService;

    public ParkingController(IParkingService parkingService)
    {
        _parkingService = parkingService;
    }

    [HttpPost("park")]
    [Authorize]
    public async Task<ActionResult<ParkingTicketResponseDTO>> ParkVehicle(
        [FromBody] ParkVehicleRequestDTO request
    )
    {
        var ticket = await _parkingService.HandleParkingEventAsync(request);

        return CreatedAtAction(
            nameof(ParkVehicle),
            new { ticketId = ticket.TicketId },
            ticket
        );
    }

    [HttpPut("exit/{ticketId:int}")]
    [Authorize]
    public async Task<ActionResult<ParkingTicketResponseDTO>> ExitVehicle(int ticketId)
    {
        var ticket = await _parkingService.HandleVehicleExitEventAsync(ticketId);

        return Ok(ticket);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ParkingTicketResponseDTO>>> GetActiveTickets()
    {
        var tickets = _parkingService.GetActiveTicketsAsync();

        return Ok(tickets);
    }

    [HttpGet("ticket/{ticketId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ParkingTicketResponseDTO>> GetTicketbyId(int ticketId)
    {
        var ticket = await _parkingService.GetTicketByIdAsync(ticketId);

        return Ok(ticket);
    }

    [HttpGet("history/{licensePlate}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ParkingTicketResponseDTO>>> GetVehicleHistory(string licensePlate)
    {
        var tickets = await _parkingService.GetVehicleHistoryAsync(licensePlate);

        return Ok(tickets);
    }
}
