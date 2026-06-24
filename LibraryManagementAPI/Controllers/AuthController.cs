/*
    To maintain SRP we should have a separate controller to handle authentication procedures.
    Authcontroller has the responsibility of: 
        - Validating JWT credentials
        - Building claims
        - Signing and returning token 
*/

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagementAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDTO dto)
    {
        if (dto.Email != "test@test.com" || dto.Password != "password123")
        {
            throw new InvalidLoginException();
        }
        
        var claims = new []
        {
            new Claim("userId", "1"),
            new Claim(ClaimTypes.Email, dto.Email),
            // new Claim(ClaimTypes.Role, "Admin")
            new Claim(ClaimTypes.Role, "User") // For testing role related status code
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["Jwt:SecretKey"]!
        ));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            )
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token)});
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult WhoAmI()
    {
        var userId = User.FindFirstValue("userid");
        var email = User.FindFirstValue(ClaimTypes.Email);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new { userId, email, role });
    }
}
