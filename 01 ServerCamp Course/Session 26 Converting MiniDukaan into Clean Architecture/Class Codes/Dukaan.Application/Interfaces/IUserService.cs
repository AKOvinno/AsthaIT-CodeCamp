using Dukaan.Application.Dtos;

namespace Dukaan.Application.Interfaces;

public interface IUserService
{
    public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
    public Task<bool> CreateMerchantAsync(MerchantDto merchant, string password);
}
