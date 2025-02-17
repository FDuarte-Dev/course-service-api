using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface ILessonRepository
{
    IEnumerable<Lesson> GetLessons();
    Lesson? GetLessonBy(int lessonId);
}

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Lesson> GetLessons()
    {
        return _context.Lessons.AsEnumerable();
    }
    
    public Lesson? GetLessonBy(int lessonId)
    {
        return _context.Lessons.FirstOrDefault(x => x.Id == lessonId);
    }
}