using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface IChapterRepository
{
    Chapter? GetChapterBy(int chapterId);
}

public class ChapterRepository : IChapterRepository
{
    private readonly AppDbContext _context;

    public ChapterRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Chapter? GetChapterBy(int chapterId)
    {
        return _context.Chapters.FirstOrDefault(x => x.Id == chapterId);
    }
}