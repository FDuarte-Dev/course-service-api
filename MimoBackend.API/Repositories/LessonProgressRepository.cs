using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;

namespace MimoBackend.API.Repositories;

public interface ILessonProgressRepository
{
    Task<LessonProgress> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username);
    Task<LessonProgress> StartLesson(string lessonId, DateTime lessonUpdate, string username);
    Task<LessonProgress> CompleteLesson(string lessonId, DateTime lessonUpdate, string username);
}

public class LessonProgressRepository : ILessonProgressRepository
{
    public Task<LessonProgress> UpdateLesson(string lessonId, LessonUpdate lessonUpdate, string username)
    {
        throw new NotImplementedException();
    }

    public Task<LessonProgress> StartLesson(string lessonId, DateTime lessonUpdate, string username)
    {
        throw new NotImplementedException();
    }

    public Task<LessonProgress> CompleteLesson(string lessonId, DateTime lessonUpdate, string username)
    {
        throw new NotImplementedException();
    }
}