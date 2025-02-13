using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IUserRepository
{
    Task<User?> GetUser(string username);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUser(string username)
    {
        return await _context.Users.FindAsync(username);
    }
}