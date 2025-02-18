using Microsoft.EntityFrameworkCore;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;

namespace MimoBackend.API.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonProgress> LessonProgresses { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public void PopulateDb()
    {
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
        var user1 = new User{ Username = "user1", Password = "password" };
        Users.Add(user1);
        
        var swiftCourse = new Course { Id = 1, Name = "Swift" };
        var javascriptCourse = new Course { Id = 2, Name = "Javascript" };
        var csharpCourse = new Course { Id = 3, Name = "C#" };
        Courses.AddRange(swiftCourse, javascriptCourse, csharpCourse);

        var swiftChapter1 = new Chapter { Order = 1, Course = swiftCourse };
        var swiftChapter2 = new Chapter { Order = 2, Course = swiftCourse };
        var javascriptChapter1 = new Chapter { Order = 1, Course = javascriptCourse };
        var javascriptChapter2 = new Chapter { Order = 2, Course = javascriptCourse };
        var csharpChapter1 = new Chapter { Order = 1, Course = csharpCourse };
        var csharpChapter2 = new Chapter { Order = 2, Course = csharpCourse };
        Chapters.AddRange(
            swiftChapter1, swiftChapter2,
            javascriptChapter1, javascriptChapter2,
            csharpChapter1, csharpChapter2);

        var swift1Lesson1 = new Lesson { Order = 1, Chapter = swiftChapter1 };
        var swift1Lesson2 = new Lesson { Order = 2, Chapter = swiftChapter1 };
        var swift2Lesson1 = new Lesson { Order = 1, Chapter = swiftChapter2 };
        var javascript1Lesson1 = new Lesson { Order = 1, Chapter = javascriptChapter1 };
        var javascript2Lesson1 = new Lesson { Order = 1, Chapter = javascriptChapter2 };
        var javascript2Lesson2 = new Lesson { Order = 2, Chapter = javascriptChapter2 };
        var javascript2Lesson3 = new Lesson { Order = 3, Chapter = javascriptChapter2 };
        var csharp1Lesson1 = new Lesson { Order = 1, Chapter = csharpChapter1 };
        var csharp1Lesson2 = new Lesson { Order = 2, Chapter = csharpChapter1 };
        var csharp1Lesson3 = new Lesson { Order = 3, Chapter = csharpChapter1 };
        var csharp2Lesson1 = new Lesson { Order = 1, Chapter = csharpChapter2 };
        var csharp2Lesson2 = new Lesson { Order = 2, Chapter = csharpChapter2 };
        Lessons.AddRange(
            swift1Lesson1, swift1Lesson2,
            swift2Lesson1,
            javascript1Lesson1,
            javascript2Lesson1, javascript2Lesson2, javascript2Lesson3,
            csharp1Lesson1, csharp1Lesson2, csharp1Lesson3,
            csharp2Lesson1, csharp2Lesson2);

        var complete5Lessons = new Achievement {Name = "Complete 5 lessons", CompletionRequirements = 5, Type = AchievementType.CompletedLessons};
        var complete25Lessons = new Achievement {Name = "Complete 25 lessons", CompletionRequirements = 25, Type = AchievementType.CompletedLessons};
        var complete50Lessons = new Achievement {Name = "Complete 50 lessons", CompletionRequirements = 50, Type = AchievementType.CompletedLessons};
        var complete1Chapter = new Achievement {Name = "Complete 1 chapter", CompletionRequirements = 1, Type = AchievementType.CompletedChapters};
        var complete5Chapters = new Achievement {Name = "Complete 5 chapters", CompletionRequirements = 5, Type = AchievementType.CompletedChapters};
        var completeSwift = new Achievement {Name = "Complete the Swift course", CompletionRequirements = 1, Type = AchievementType.CompletedCourses};
        var completeJavascript = new Achievement {Name = "Complete the Javascript course", CompletionRequirements = 1, Type = AchievementType.CompletedCourses};
        var completeCsharp = new Achievement {Name = "Complete the C# course", CompletionRequirements = 1, Type = AchievementType.CompletedCourses};
        Achievements.AddRange(
            complete5Lessons, complete25Lessons, complete50Lessons,
            complete1Chapter, complete5Chapters,
            completeSwift, completeJavascript, completeCsharp);

        SaveChanges();
    }
}
