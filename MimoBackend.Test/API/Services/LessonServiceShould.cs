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

    private readonly LessonService _service;

    public LessonServiceShould()
    {
        _service = new LessonService(_lessonRepository.Object);
    }

    #region GetLessonBy

    [Fact]
    public void ReturnChapterIfItExists()
    {
        // Arrange
        var expectedLesson = new Lesson(){Id = 1};
        
        _lessonRepository.Setup(x => x.GetLessonBy(1))
            .Returns(expectedLesson);
        
        // Act
        var result = _service.GetLessonBy(1);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Fact]
    public void ReturnNotFoundChapterIfItDoesNotExist()
    {
        // Arrange
        var expectedLesson = NotFoundLesson.GetNotFoundLesson();
        
        _lessonRepository.Setup(x => x.GetLessonBy(1))
            .Returns((Lesson?)null);
        
        // Act
        var result = _service.GetLessonBy(1);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedLesson);
    }

    #endregion

    #region GetLessons

    [Fact]
    public void ReturnSuccessWithEmptyListOnNoLessonsFound()
    {
        // Arrange
        _lessonRepository.Setup(x => x.GetLessons())
            .Returns([]);
        
        // Act
        var result = _service.GetLessons();

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void ReturnSuccessLessonsList()
    {
        // Arrange
        var lessons = new List<Lesson> { new() { Id = 1 } };
        _lessonRepository.Setup(x => x.GetLessons())
            .Returns(lessons);
        
        // Act
        var result = _service.GetLessons();

        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
    }

    #endregion
}