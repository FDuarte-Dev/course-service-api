namespace MimoBackend.API.Models;

public class AuthenticationToken
{
    public string Token { get; set; }

    public string Username { get; set; }
    public long Expires { get; set; }
}