public class UserController
{
    [HttpGet("/user")]
    public string GetUser()
    {
        return "Fetching user from controller";
    }
}