namespace ParkingManagementAPI.DTOs;

public interface IAuthService
{
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto);
}
