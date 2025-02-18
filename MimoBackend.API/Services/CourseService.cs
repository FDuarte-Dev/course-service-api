using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;

namespace MimoBackend.API.Services;

public interface ICourseService
{
    Course GetCourseBy(int courseId);
    bool UserCompletedCourse(Course course, User user);
}

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    
    public Course GetCourseBy(int courseId)
    {
        return _courseRepository.GetCourseBy(courseId) ?? NotFoundCourse.GetNotFoundCourse();
    }

    public bool UserCompletedCourse(Course course, User user)
    {
        throw new NotImplementedException();
    }
}