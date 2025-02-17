using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class LessonServiceShould
{
    private readonly Mock<ILessonRepository> _lessonRepository = new();

    private const string Username = "user1";

    private readonly LessonService _service;

    public LessonServiceShould()
    {
        _service = new LessonService(_lessonRepository.Object);
    }

    [Fact]
    public void ReturnSuccessWithEmptyListOnNoLessonsFound()
    {
        // Arrange
        _lessonRepository.Setup(x => x.GetLessons())
            .Returns([]);
        
        // Act
        var result = _service.GetLessons(Username);

        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("[]");
    }
    
    [Fact]
    public void ReturnSuccessLessonsList()
    {
        // Arrange
        var lessons = new List<Lesson> { new() { Id = 1 } };
        _lessonRepository.Setup(x => x.GetLessons())
            .Returns(lessons);
        
        // Act
        var result = _service.GetLessons(Username);

        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("\"Id\":1");
    }
}