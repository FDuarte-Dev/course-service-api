using Microsoft.EntityFrameworkCore;
using MimoBackend.API.Models.DTOs;

namespace MimoBackend.API.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<UserDto> Users { get; set; }
    public DbSet<CourseDto> Courses { get; set; }
    public DbSet<ChapterDto> Chapters { get; set; }
    public DbSet<LessonDto> Lessons { get; set; }
    public DbSet<LessonProgressDto> LessonProgresses { get; set; }
    public DbSet<AchievementDto> Achievements { get; set; }
    public DbSet<UserAchievementsDto> UserAchievements { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
        ClearContext();
        PopulateContext();
    }

    
        private void ClearContext()
    {
        Users.RemoveRange(Users);
        Courses.RemoveRange(Courses);
        Chapters.RemoveRange(Chapters);
        Lessons.RemoveRange(Lessons);
        Achievements.RemoveRange(Achievements);
        LessonProgresses.RemoveRange(LessonProgresses);
        UserAchievements.RemoveRange(UserAchievements);
        SaveChanges();
    }
    
    private void PopulateContext()
    {
        var user1 = new UserDto{ Username = "user1", Password = "password" };
        Users.Add(user1);
        
        var swiftCourse = new CourseDto { Id = 1, Name = "Swift" };
        var javascriptCourse = new CourseDto { Id = 2, Name = "Javascript" };
        var csharpCourse = new CourseDto { Id = 3, Name = "C#" };
        Courses.AddRange(swiftCourse, javascriptCourse, csharpCourse);

        var swiftChapter1 = new ChapterDto { Order = 1, Course = swiftCourse };
        var swiftChapter2 = new ChapterDto { Order = 2, Course = swiftCourse };
        var javascriptChapter1 = new ChapterDto { Order = 1, Course = javascriptCourse };
        var javascriptChapter2 = new ChapterDto { Order = 2, Course = javascriptCourse };
        var csharpChapter1 = new ChapterDto { Order = 1, Course = csharpCourse };
        var csharpChapter2 = new ChapterDto { Order = 2, Course = csharpCourse };
        Chapters.AddRange(
            swiftChapter1, swiftChapter2,
            javascriptChapter1, javascriptChapter2,
            csharpChapter1, csharpChapter2);

        var swift1Lesson1 = new LessonDto { Order = 1, Chapter = swiftChapter1 };
        var swift1Lesson2 = new LessonDto { Order = 2, Chapter = swiftChapter1 };
        var swift2Lesson1 = new LessonDto { Order = 1, Chapter = swiftChapter2 };
        var javascript1Lesson1 = new LessonDto { Order = 1, Chapter = javascriptChapter1 };
        var javascript2Lesson1 = new LessonDto { Order = 1, Chapter = javascriptChapter2 };
        var javascript2Lesson2 = new LessonDto { Order = 2, Chapter = javascriptChapter2 };
        var javascript2Lesson3 = new LessonDto { Order = 3, Chapter = javascriptChapter2 };
        var csharp1Lesson1 = new LessonDto { Order = 1, Chapter = csharpChapter1 };
        var csharp1Lesson2 = new LessonDto { Order = 2, Chapter = csharpChapter1 };
        var csharp1Lesson3 = new LessonDto { Order = 3, Chapter = csharpChapter1 };
        var csharp2Lesson1 = new LessonDto { Order = 1, Chapter = csharpChapter2 };
        var csharp2Lesson2 = new LessonDto { Order = 2, Chapter = csharpChapter2 };
        Lessons.AddRange(
            swift1Lesson1, swift1Lesson2,
            swift2Lesson1,
            javascript1Lesson1,
            javascript2Lesson1, javascript2Lesson2, javascript2Lesson3,
            csharp1Lesson1, csharp1Lesson2, csharp1Lesson3,
            csharp2Lesson1, csharp2Lesson2);

        var complete5Lessons = new AchievementDto {Name = "Complete 5 lessons", CompletionRequirements = 5};
        var complete25Lessons = new AchievementDto {Name = "Complete 25 lessons", CompletionRequirements = 25};
        var complete50Lessons = new AchievementDto {Name = "Complete 50 lessons", CompletionRequirements = 50};
        var complete1Chapter = new AchievementDto {Name = "Complete 1 chapter", CompletionRequirements = 1};
        var complete5Chapters = new AchievementDto {Name = "Complete 5 chapters", CompletionRequirements = 5};
        var completeSwift = new AchievementDto {Name = "Complete the Swift course", CompletionRequirements = 1};
        var completeJavascript = new AchievementDto {Name = "Complete the Javascript course", CompletionRequirements = 1};
        var completeCsharp = new AchievementDto {Name = "Complete the C# course", CompletionRequirements = 1};
        Achievements.AddRange(
            complete5Lessons, complete25Lessons, complete50Lessons,
            complete1Chapter, complete5Chapters,
            completeSwift, completeJavascript, completeCsharp);

        SaveChanges();
    }
}
