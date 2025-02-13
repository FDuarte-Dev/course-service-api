using MimoBackend.API.Models.DTOs;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IUserRepository
{
    Task<UserDto?> GetUser(string username);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUser(string username)
    {
        return await _context.Users.FindAsync(username);
    }
}