using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface ILessonRepository
{
    IEnumerable<Lesson> GetLessons();
    Lesson? GetLessonBy(int lessonId);
    IEnumerable<Lesson> GetChapterLessons(int chapterId);
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
        => _context.Lessons.FirstOrDefault(x => x.Id == lessonId);

    public IEnumerable<Lesson> GetChapterLessons(int chapterId) 
        => _context.Lessons.Where(x => x.ChapterId == chapterId);
}