using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IChapterService
{
    Chapter GetChapterBy(int chapterId);
    bool UserCompletedChapter(Chapter chapter, User user);
}

public class ChapterService : IChapterService
{
    private readonly IChapterRepository _chapterRepository;

    public ChapterService(IChapterRepository chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }
    
    public Chapter GetChapterBy(int chapterId)
    {
        return _chapterRepository.GetChapterBy(chapterId) ?? NotFoundChapter.GetNotFoundChapter();
    }

    public bool UserCompletedChapter(Chapter chapter, User user)
    {
        throw new NotImplementedException();
    }
}