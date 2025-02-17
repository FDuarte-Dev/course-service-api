using Microsoft.EntityFrameworkCore;
using MimoBackend.API.Persistence;
using MimoBackend.API.Persistence.Caches;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;

namespace MimoBackend.API;

public static class ServiceCollectionHelper
{
    public static void SetServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<ILessonProgressService, LessonProgressService>();
        services.AddScoped<IUserAchievementService, UserAchievementService>();
    }
    
    public static void SetRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
    }
    
    public static void SetPersistence(this IServiceCollection services)
    { 
        services.AddDbContextFactory<AppDbContext>(
            options => options.UseSqlite($"Data Source={Directory.GetCurrentDirectory() + "/Persistence/mimo.db"}"));
        services.AddSingleton<ITokenCache, TokenCache>();
    }
}