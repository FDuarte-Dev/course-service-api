using MimoBackend.API.Models;
using MimoBackend.API.Models.DTOs;

namespace MimoBackend.API.Repositories;

public interface IUserRepository
{
    Task<UserDto?> GetUser(string username);
}

public class UserRepository : IUserRepository
{
    public Task<UserDto?> GetUser(string username)
    {
        throw new NotImplementedException();
    }
}