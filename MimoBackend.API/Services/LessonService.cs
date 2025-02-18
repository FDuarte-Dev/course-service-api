using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ILessonService
{
    Lesson GetLessonBy(int lessonId);
    IEnumerable<Lesson> GetLessons();
    IEnumerable<Lesson> GetChapterLessons(int chapterId);
}

public class LessonService : BaseService, ILessonService
{
    private readonly ILessonRepository _lessonRepository;

    public LessonService(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public Lesson GetLessonBy(int lessonId) 
        => _lessonRepository.GetLessonBy(lessonId) ?? NotFoundLesson.GetNotFoundLesson();

    public IEnumerable<Lesson> GetLessons()
    {
        var lessons = _lessonRepository.GetLessons();
        return lessons;
    }

    public IEnumerable<Lesson> GetChapterLessons(int chapterId)
    {
        return _lessonRepository.GetChapterLessons(chapterId);
    }
}