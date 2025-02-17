using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence;

namespace MimoBackend.API.Repositories;

public interface ICourseRepository
{
    Course? GetCourseBy(int courseId);
}

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Course? GetCourseBy(int courseId)
    {
        return _context.Courses.FirstOrDefault(x => x.Id == courseId);
    }
}