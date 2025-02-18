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
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ILessonProgressService, LessonProgressService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IUserAchievementService, UserAchievementService>();
        services.AddScoped<IUserService, UserService>();
    }
    
    public static void SetRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
    
    public static void SetPersistence(this IServiceCollection services)
    { 
        services.AddDbContextFactory<AppDbContext>(
            options => options.UseSqlite($"Data Source={Directory.GetCurrentDirectory() + "/Persistence/mimo.db"}"));
        services.AddSingleton<ITokenCache, TokenCache>();
    }
}