using MiniDukaan.Application.DTOs;

namespace MiniDukaan.Application.Interfaces;

public interface IUserService
{
    public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
    public Task<bool> CreateMerchantAsync(MerchantDTO merchant, string password);
}