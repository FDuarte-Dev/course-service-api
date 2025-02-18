using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IChapterRepository
{
    Chapter? GetChapterBy(int chapterId);
    IEnumerable<Chapter> GetCourseChapters(int courseId);
}

public class ChapterRepository : IChapterRepository
{
    private readonly AppDbContext _context;

    public ChapterRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Chapter? GetChapterBy(int chapterId) 
        => _context.Chapters.FirstOrDefault(x => x.Id == chapterId);

    public IEnumerable<Chapter> GetCourseChapters(int courseId) 
        => _context.Chapters.Where(x => x.CourseId == courseId);
}