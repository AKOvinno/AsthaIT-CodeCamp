using Microsoft.AspNetCore.Mvc;
using MiniDukaan.Application.DTOs;
using MiniDukaan.Infrastructure.Services;

namespace MiniDukaan.Host.Controllers;

[ApiController]
[Route("api/[controller]")] // domain/api/tenants
public class TenantsController(TenantService tenantService) : ControllerBase
{
    private readonly TenantService _tenantService = tenantService;

    [HttpPost("register")] // domain/api/tenants/register
    public async Task<ActionResult> Register(MerchantRegisterRequest request)
    {
        var response = await _tenantService.RegisterMerchant(request);
        return Ok(response);
    }
}
