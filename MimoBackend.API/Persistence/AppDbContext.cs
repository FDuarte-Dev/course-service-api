using Microsoft.EntityFrameworkCore;
using MimoBackend.API.Models.DTOs;

namespace MimoBackend.API.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<UserDto> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}