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
    private readonly Mock<ILessonRepository> _lessonRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ILessonProgressRepository> _lpRepository = new();
    
    private const string LessonId = "1";
    private const string Username = "user1";

    private readonly User _user = new ()
    {
        Username = "usr",
        Password = "pwd"
    };
    private readonly Lesson _lesson = new ()
    {
        Id = 1,
        Order = 1
    };
    private readonly LessonUpdate _lessonUpdate = new()
    {
        StartTime = DateTime.Today,
        CompletionTime = DateTime.Today
    };

    private readonly LessonProgressService _service;

    public LessonProgressServiceShould()
    {
        _lessonRepository.Setup(x => x.GetLessonBy(LessonId))
            .Returns(_lesson);
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns(_user);
        
        _service = new LessonProgressService(
            _lessonRepository.Object,
            _userRepository.Object,
            _lpRepository.Object);
    }

    [Fact]
    public void CreateNewLessonProgressOnUpdateLessonProgress()
    {
        // Arrange
        var expectedLessonUpdate = new LessonProgress()
        {
            LessonId = int.Parse(LessonId),
            UserUsername = Username,
            StartTime = _lessonUpdate.StartTime,
            CompletionTime = _lessonUpdate.CompletionTime
        };
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(expectedLessonUpdate);
        
        // Act
        var result = _service.UpdateLesson(LessonId, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void ReturnNotFoundIfMissingLesson()
    {
        // Arrange
        _lessonRepository.Setup(x => x.GetLessonBy(LessonId))
            .Returns((Lesson?)null);
        
        // Act
        var result = _service.UpdateLesson(LessonId, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public void ReturnNotFoundIfMissingUser()
    {
        // Arrange
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns((User?)null);
        
        // Act
        var result = _service.UpdateLesson(LessonId, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public void UpdateStartDateWhenReStartingNonCompletedLesson()
    {
        // Arrange
        // Act
        // Assert
    }
    
    [Fact]
    public void CreateNewLessonProgressWhenReStartingCompletedLesson()
    {
        // Arrange
        // Act
        // Assert
    }
    
    [Fact]
    public void CompleteLessonProgress()
    {
        // Arrange
        // Act
        // Assert
    }
    
    [Fact]
    public void ReturnNotFoundOnNonexistentLessonProgressOnCompleteLesson()
    {
        // Arrange
        // Act
        // Assert
    }
}