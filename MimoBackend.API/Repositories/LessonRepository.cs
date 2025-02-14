using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface ILessonRepository
{
    Lesson? GetLessonBy(string lessonId);
}

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Lesson? GetLessonBy(string lessonId)
    {
        return _context.Lessons.Find(lessonId);
    }
}