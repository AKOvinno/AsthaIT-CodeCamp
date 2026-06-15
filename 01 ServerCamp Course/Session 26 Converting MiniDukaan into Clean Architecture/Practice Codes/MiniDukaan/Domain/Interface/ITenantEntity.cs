namespace MiniDukaan.Domain.Interface;
// Which products are tenantable they must inherit ITenantEntity
public interface ITenantEntity
{
    Guid TenantId { get; set; }
}