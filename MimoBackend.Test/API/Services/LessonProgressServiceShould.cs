using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class LessonProgressServiceShould
{
    private readonly Mock<ILessonProgressRepository> _lpRepository = new();
    
    private const string LessonId = "1";
    private const string Username = "user1";
    private readonly LessonUpdate _lessonUpdate = new()
    {
        StartTime = DateTime.Today,
        CompletionTime = DateTime.Today
    };

    private readonly LessonProgressService _service;

    public LessonProgressServiceShould()
    {
        _service = new LessonProgressService(_lpRepository.Object);
    }

    [Fact]
    public async Task UpdateLessonProgress()
    {
        // Arrange
        var expectedLessonUpdate = new LessonProgress()
        {
            LessonId = int.Parse(LessonId),
            UserUsername = Username,
            StartTime = _lessonUpdate.StartTime,
            CompletionTime = _lessonUpdate.CompletionTime
        };
        _lpRepository.Setup(x => x.UpdateLesson(LessonId, _lessonUpdate, Username))
            .ReturnsAsync(expectedLessonUpdate);
        
        // Act
        var result = await _service.UpdateLesson(LessonId, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task StartNewLessonProgress()
    {
        // Arrange
        var expectedLessonUpdate = new LessonProgress()
        {
            LessonId = int.Parse(LessonId),
            UserUsername = Username,
            StartTime = _lessonUpdate.StartTime,
        };
        _lpRepository.Setup(x => x.StartLesson(LessonId, DateTime.Today, Username))
            .ReturnsAsync(expectedLessonUpdate);
        
        // Act
        var result = await _service.StartLesson(LessonId, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task StartMultipleLessonProgresses()
    {
        // Arrange
        var expectedLessonUpdate = new LessonProgress()
        {
            LessonId = int.Parse(LessonId),
            UserUsername = Username,
            StartTime = _lessonUpdate.StartTime,
        };
        _lpRepository.Setup(x => x.StartLesson(LessonId, DateTime.Today, Username))
            .ReturnsAsync(expectedLessonUpdate);
        
        var expectedLessonUpdate2 = new LessonProgress()
        {
            LessonId = 2,
            UserUsername = Username,
            StartTime = _lessonUpdate.StartTime,
        };
        _lpRepository.Setup(x => x.StartLesson("2", DateTime.Today, Username))
            .ReturnsAsync(expectedLessonUpdate2);
        
        // Act
        var result = await _service.StartLesson(LessonId, DateTime.Today, Username);
        var result2 = await _service.StartLesson("2", DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result2 as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task CompleteLessonProgress()
    {
        // Arrange
        var expectedLessonUpdate = new LessonProgress()
        {
            LessonId = int.Parse(LessonId),
            UserUsername = Username,
            StartTime = _lessonUpdate.StartTime,
        };
        _lpRepository.Setup(x => x.CompleteLesson(LessonId, DateTime.Today, Username))
            .ReturnsAsync(expectedLessonUpdate);
        
        // Act
        var result = await _service.CompleteLesson(LessonId, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task ReturnNotFoundOnUpdateMissingLesson()
    {
        // Arrange
        _lpRepository.Setup(x => x.UpdateLesson(LessonId, _lessonUpdate, Username))
            .ReturnsAsync((LessonProgress)null!);
        
        // Act
        var result = await _service.UpdateLesson(LessonId, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public async Task ReturnNotFoundOnStartMissingLesson()
    {
        // Arrange
        _lpRepository.Setup(x => x.StartLesson(LessonId, DateTime.Today, Username))
            .ReturnsAsync((LessonProgress)null!);
        
        // Act
        var result = await _service.StartLesson(LessonId, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public async Task ReturnNotFoundOnCompleteMissingLesson()
    {
        // Arrange
        _lpRepository.Setup(x => x.CompleteLesson(LessonId, DateTime.Today, Username))
            .ReturnsAsync((LessonProgress)null!);
        
        // Act
        var result = await _service.CompleteLesson(LessonId, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}