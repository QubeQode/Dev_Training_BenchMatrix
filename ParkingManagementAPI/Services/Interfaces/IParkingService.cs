using ParkingManagementAPI.DTOs;

namespace ParkingManagementAPI.Services;

public interface IParkingService
{
    Task<ParkingTicketResponseDTO> HandleParkingEventAsync(ParkVehicleRequestDTO request);

    Task<ParkingTicketResponseDTO> HandleVehicleExitEventAsync (int ticketId);

    Task<IEnumerable<ParkingTicketResponseDTO>> GetActiveTicketsAsync();

    Task<ParkingTicketResponseDTO> GetTicketByIdAsync(int ticketId);

    Task<IEnumerable<ParkingTicketResponseDTO>> GetVehicleHistoryAsync(string licensePlate);
}
