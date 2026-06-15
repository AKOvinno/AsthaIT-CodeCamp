using Microsoft.AspNetCore.Identity;
using MiniDukaan.Domain.Interface;
namespace MiniDukaan.Infrastructure.Data.Model;
public class Merchant : IdentityUser<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}