using System.ComponentModel.DataAnnotations;

namespace ParkingManagementAPI.DTOs;

public class LoginRequestDTO
{
    [Required]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
