public class User
{
    public Guid Id { get; set; } // If EF gets Id, then it will define it as PK automatically, If we define UserId then also it will be PK. If we use UserKey then it will not be PK automatically. In that case we have define attributs [Key] explicitly at top to make it PK
    public string? Name { get; set; }
    public string? Email { get; set; }

}