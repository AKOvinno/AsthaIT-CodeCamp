namespace MiniDukaan.Application.DTOs;

// public class RegisterRequest
// {
    
// }

// Instead of class we should use record because class goes to heap and record is lightweight than class. DTOs doesn't have any methods it just for holding some properties that's why we should use record because it's lightweight and doesn't use heavy memory. DTOs doesn't get inherited normally. It's a pure CLR Object. Pure CLR object means it's not inherited from anywhere
public record MerchantRegisterRequest(
    string Email,
    string PhoneNumber,
    string Password,
    string StoreName,
    string Slug,
    string Category,
    string Country
);
public record RegisterResponse(
    Guid TenantId,
    string StoreName
);