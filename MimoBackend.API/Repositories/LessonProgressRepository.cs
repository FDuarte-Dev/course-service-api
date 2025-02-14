using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface ILessonProgressRepository
{
    LessonProgress AddLessonProgress(LessonProgress lessonProgress);
    LessonProgress? UpdateLessonProgressStartTime(int lessonProgressId, DateTime startTime);
    LessonProgress? UpdateLessonProgressCompletionTime(int lessonProgressId, DateTime completionTime);
    List<LessonProgress> FindByLessonAndUser(Lesson? lesson, User? user);
    LessonProgress? FindByLessonUserAndCompletion(Lesson? lesson, User? user, bool completed);
}

public class LessonProgressRepository : ILessonProgressRepository
{
    private readonly AppDbContext _context;

    public LessonProgressRepository(AppDbContext context)
    {
        _context = context;
    }

    private LessonProgress? GetLessonProgressById(int lessonProgressId)
    {
        return _context.LessonProgresses.Find(lessonProgressId);
    }

    public List<LessonProgress> FindByLessonAndUser(Lesson? lesson, User? user)
    {
        return _context.LessonProgresses
            .Where(x => 
                x.LessonId == lesson.Id &&
                x.UserUsername == user.Username)
            .ToList();
    }

    public LessonProgress? FindByLessonUserAndCompletion(Lesson? lesson, User? user, bool completed)
    {
        return _context.LessonProgresses
            .Where(x => 
                x.LessonId == lesson.Id &&
                x.UserUsername == user.Username &&
                x.CompletionTime.HasValue == completed)
            .ToList()
            .FirstOrDefault();
    }



    public LessonProgress AddLessonProgress(LessonProgress lessonProgress)
    {
        var entry = _context.LessonProgresses.Add(lessonProgress);
        _context.SaveChanges();
        return entry.Entity;
    }

    public LessonProgress UpdateLessonProgressStartTime(int lessonProgressId, DateTime startTime)
    {
        var lessonProgress = GetLessonProgressById(lessonProgressId);
        lessonProgress!.StartTime = startTime;
        var entry = _context.LessonProgresses.Update(lessonProgress);
        _context.SaveChanges();
        return entry.Entity;
    }
    
    public LessonProgress UpdateLessonProgressCompletionTime(int lessonProgressId, DateTime completionTime)
    {
        var lessonProgress = GetLessonProgressById(lessonProgressId);
        lessonProgress!.CompletionTime = completionTime;
        var entry = _context.LessonProgresses.Update(lessonProgress);
        _context.SaveChanges();
        return entry.Entity;
    }
}