using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface ILessonProgressRepository
{
    LessonProgress AddLessonProgress(LessonProgress lessonProgress);
    LessonProgress UpdateLessonProgress(LessonProgress lessonProgress);
    List<LessonProgress> FindByLessonAndUser(Lesson? lesson, User? user);
    List<LessonProgress> FindByLessonUserAndCompletion(Lesson? lesson, User? user, bool completed);
}

public class LessonProgressRepository : ILessonProgressRepository
{
    private readonly AppDbContext _context;

    public LessonProgressRepository(AppDbContext context)
    {
        _context = context;
    }

    public LessonProgress AddLessonProgress(LessonProgress lessonProgress)
    {
        return _context.LessonProgresses.Add(lessonProgress).Entity;
    }

    public LessonProgress UpdateLessonProgress(LessonProgress lessonProgress)
    {
        return _context.LessonProgresses.Update(lessonProgress).Entity;
    }

    public List<LessonProgress> FindByLessonAndUser(Lesson? lesson, User? user)
    {
        return _context.LessonProgresses
            .Where(x => 
                x.LessonId == lesson.Id &&
                x.UserUsername == user.Username)
            .ToList();
    }

    public List<LessonProgress> FindByLessonUserAndCompletion(Lesson? lesson, User? user, bool completed)
    {
        return _context.LessonProgresses
            .Where(x => 
                x.LessonId == lesson.Id &&
                x.UserUsername == user.Username &&
                !x.CompletionTime.HasValue)
            .ToList();
    }
}