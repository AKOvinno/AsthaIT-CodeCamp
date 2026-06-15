using MiniDukaan.Application.DTOs;

namespace MiniDukaan.Application.Interfaces;

/// <summary>
/// Defines methods for authenticating users and managing authentication operations.
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
}