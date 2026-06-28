using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Exceptions;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService (IConfiguration config)
    {
        _config = config;
    }

    public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto)
    {
        if (dto.Email != "test@test.com" || dto.Password != "password123")
        {
            throw new InvalidCredentialsException();
        }
        
        var claims = new []
        {
            new Claim("userId", "1"),
            new Claim(ClaimTypes.Email, dto.Email),
            new Claim(ClaimTypes.Role, "Admin")
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

        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);
        
        return Task.FromResult(
            new LoginResponseDTO
            {
                Token = tokenString
            }
        );
    }
}
