using Microsoft.AspNetCore.Mvc;
using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Services;

namespace ParkingManagementAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO dto)
    {
        var response = await _authService.LoginAsync(dto);

        return Ok(response);
    }
}
