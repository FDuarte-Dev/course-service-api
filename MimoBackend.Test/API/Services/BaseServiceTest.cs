using MimoBackend.API.Models.DatabaseObjects;

namespace MimoBackend.Test.API.Services;

public class BaseServiceTest
{
    protected const string Username = "user1";
    
    protected readonly User User = new ()
    {
        Username = Username,
        Password = "pwd"
    };
}