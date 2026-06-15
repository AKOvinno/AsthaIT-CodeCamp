using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniDukaan.Application.DTOs;
using MiniDukaan.Application.Interfaces;
using MiniDukaan.Infrastructure.Data.Model;

namespace MiniDukaan.Infrastructure.Data.Services;

public class UserService : IUserService
{
    private readonly UserManager<Merchant> _userManager;
    private readonly IConfiguration _config;

    // Much cleaner! Just inject what YOUR service actually interacts with.
    public UserService(UserManager<Merchant> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        // Use the injected _userManager instance instead of 'this'
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isValid)
            throw new UnauthorizedAccessException("Invalid credentials");

        var jwt = GenerateToken(user);

        var minutes = _config["jwt:ExpireInMinutes"];
        var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(minutes!));

        return new AuthResponseDTO(jwt, expiresAt);
    }

    public async Task<bool> CreateMerchantAsync(MerchantDTO merchantDto, string password)
    {
        var merchant = new Merchant
        {
            UserName = merchantDto.Email,
            Email = merchantDto.Email,
            PhoneNumber = merchantDto.PhoneNumber,
            TenantId = merchantDto.TenantId
        };

        var result = await _userManager.CreateAsync(merchant, password);
        return result.Succeeded;
    }

    private string GenerateToken(Merchant user)
    {
        var claims = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim("email", user.Email!),
            new Claim("tenant_id", user.TenantId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["jwt:ExpireInMinutes"]!)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256)
        };

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(securityToken);
    }
}