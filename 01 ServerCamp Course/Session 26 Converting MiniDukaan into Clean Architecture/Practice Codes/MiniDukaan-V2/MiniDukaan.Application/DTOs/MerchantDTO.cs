namespace MiniDukaan.Application.DTOs;

public class MerchantDTO
{
    public string UserName { get; set; } = string.Empty;  // avoid null warnings
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid TenantId { get; set; } = Guid.Empty;
}
