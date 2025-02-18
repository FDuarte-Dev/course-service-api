using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface IChapterService
{
    Chapter GetChapterBy(int chapterId);
    bool UserCompletedChapter(Chapter chapter, User user);
    IEnumerable<Chapter> GetCourseChapters(int courseId);
}

public class ChapterService : BaseService, IChapterService
{
    private readonly ILessonService _lessonService;
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IChapterRepository _chapterRepository;

    public ChapterService(
        ILessonService lessonService,
        ILessonProgressRepository lessonProgressRepository,
        IChapterRepository chapterRepository)
    {
        _lessonService = lessonService;
        _lessonProgressRepository = lessonProgressRepository;
        _chapterRepository = chapterRepository;
    }
    
    public Chapter GetChapterBy(int chapterId)
    {
        return _chapterRepository.GetChapterBy(chapterId) ?? NotFoundChapter.GetNotFoundChapter();
    }

    public bool UserCompletedChapter(Chapter chapter, User user)
    {
        var chapterLessons = _lessonService.GetChapterLessons(chapter.Id)
            .ToList();
        return chapterLessons.Count > 0 && CompletedAllLessons(chapterLessons, user);
    }

    public IEnumerable<Chapter> GetCourseChapters(int courseId)
    {
        return _chapterRepository.GetCourseChapters(courseId);
    }

    private bool CompletedAllLessons(IEnumerable<Lesson> chapterLessons, User user) 
        => chapterLessons.All(x =>
            _lessonProgressRepository.FindByLessonUserAndCompletion(x, user, true) is not null);
}