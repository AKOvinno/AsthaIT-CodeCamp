using MiniDukaan.Application.DTOs;
using MiniDukaan.Application.Interfaces;
using MiniDukaan.Domain.Entities;

namespace MiniDukaan.Application.Services;

public class TenantService(
        IRepository<Tenant> tenantRepository,
        IUserService userService
    )
{
    public async Task<RegisterResponse> RegisterMerchant(MerchantRegisterRequest request)
    {
        var tenant = new Tenant
        {
            StoreName = request.StoreName,
            Slug = request.Slug.ToLower(),
            Category = request.Category,
            Country = request.Country
        };

        await tenantRepository.AddAsync(tenant);
        await tenantRepository.SaveChangesAsync();

        var merchantDto = new MerchantDTO
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            TenantId = tenant.Id
        };

        var isMerchantCreated = await userService.CreateMerchantAsync(merchantDto, request.Password);

        return !isMerchantCreated
            ? throw new Exception("Merchant creation failed")
            : new RegisterResponse(tenant.Id, tenant.StoreName);
    }
}