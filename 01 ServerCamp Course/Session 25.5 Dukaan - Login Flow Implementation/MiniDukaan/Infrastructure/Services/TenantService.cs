using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using MiniDukaan.Application.DTOs;
using MiniDukaan.Domain.Entities;
using MiniDukaan.Infrastructure.Data.Model;

namespace MiniDukaan.Infrastructure.Services;

public class TenantService(
        Repository<Tenant> tenantRepository,
        UserManager<Merchant> userManager
    )
{
    public readonly UserManager<Merchant> _userManager = userManager;
    private readonly Repository<Tenant> _tenantRepository = tenantRepository;
    public async Task<RegisterResponse> RegisterMerchant(MerchantRegisterRequest request)
    {
        var tenant = new Tenant
        {
            StoreName = request.StoreName,
            Slug = request.Slug.ToLower(),
            Category = request.Category,
            Country = request.Country
        };
        await _tenantRepository.AddAsync(tenant);
        await _tenantRepository.SaveChangesAsync();

        var merchant = new Merchant
        {
            UserName = request.Email.Split('@')[0],
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            TenantId = tenant.Id
        };
        var result = await _userManager.CreateAsync(merchant, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception("Merchant creation failed");
        }
        return new RegisterResponse(
            tenant.Id,
            tenant.StoreName
        );
    }
}