using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ICourseService
{
    Course GetCourseBy(int courseId);
    bool UserCompletedCourse(Course course, User user);
}

public class CourseService : BaseService, ICourseService
{
    private readonly IChapterService _chapterService;
    private readonly ICourseRepository _courseRepository;

    public CourseService(
        IChapterService chapterService,
        ICourseRepository courseRepository)
    {
        _chapterService = chapterService;
        _courseRepository = courseRepository;
    }
    
    public Course GetCourseBy(int courseId)
    {
        return _courseRepository.GetCourseBy(courseId) ?? NotFoundCourse.GetNotFoundCourse();
    }

    public bool UserCompletedCourse(Course course, User user)
    {
        var chapters = _chapterService.GetCourseChapters(course.Id);
        return chapters.All(x => _chapterService.UserCompletedChapter(x, user));
    }
}