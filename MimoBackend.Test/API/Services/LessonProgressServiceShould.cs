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
    private readonly Mock<IChapterService> _chapterService = new();
    private readonly Mock<ICourseService> _courseService = new();
    private readonly Mock<IUserAchievementService> _userAchievementService = new();
    private readonly Mock<ILessonService> _lessonService = new();
    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILessonProgressRepository> _lpRepository = new();
    
    private const int LessonProgressId = 1;
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
        _lessonService.Setup(x => x.GetLessonBy(_lesson.Id))
            .Returns(_lesson);
        _userService.Setup(x => x.GetUserBy(Username))
            .Returns(_user);
        
        _service = new LessonProgressService(
            _chapterService.Object,
            _courseService.Object,
            _userAchievementService.Object,
            _lessonService.Object,
            _userService.Object,
            _lpRepository.Object);
    }

    #region UpdateLesson

    [Fact]
    public void CreateNewLessonProgressOnUpdateLessonProgress()
    {
        // Arrange
        var expectedLessonUpdate =
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(expectedLessonUpdate);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void ReturnNotFoundIfMissingLesson()
    {
        // Arrange
        _lessonService.Setup(x => x.GetLessonBy(_lesson.Id))
            .Returns((Lesson?)null);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public void ReturnNotFoundIfMissingUser()
    {
        // Arrange
        _userService.Setup(x => x.GetUserBy(Username))
            .Returns((User?)null);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    #endregion

    #region StartLesson

    [Fact]
    public void UpdateStartDateWhenReStartingNonCompletedLesson()
    {
        // Arrange
        var existing = 
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.FindByLessonUserAndCompletion(_lesson, _user, false))
            .Returns(existing);
        _lpRepository.Setup(x => x.UpdateLessonProgressStartTime(1, _lessonUpdate.StartTime))
            .Returns(existing);
        
        // Act
        var result = _service.StartLesson(_lesson.Id, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("\"Id\":1");
    }
    
    [Fact]
    public void CreateNewLessonProgressWhenReStartingCompletedLesson()
    {
        // Arrange
        var lessonProgress = 
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.FindByLessonUserAndCompletion(_lesson, _user, true))
            .Returns((LessonProgress?)null);
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(lessonProgress);
        
        // Act
        var result = _service.StartLesson(_lesson.Id, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("\"Id\":1");
    }

    #endregion

    #region CompleteLesson

    [Fact]
    public void CompleteLessonProgress()
    {
        // Arrange
        var existing = 
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.FindByLessonUserAndCompletion(_lesson, _user, false))
            .Returns(existing);
        _lpRepository.Setup(x => x.UpdateLessonProgressCompletionTime(1, _lessonUpdate.StartTime))
            .Returns(existing);
        
        // Act
        var result = _service.CompleteLesson(_lesson.Id, DateTime.Today, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("\"Id\":1");
    }
    
    [Fact]
    public void ReturnNotFoundOnNonexistentLessonProgressOnCompleteLesson()
    {
        // Arrange
        _lpRepository.Setup(x => x.FindByLessonUserAndCompletion(_lesson, _user, false))
            .Returns((LessonProgress?)null);
        
        // Act
        var result = _service.CompleteLesson(_lesson.Id, _lessonUpdate.CompletionTime, Username);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

    }

    #endregion

    #region UserAchievementUpdates

    [Fact]
    public void UpdateLessonUserAchievementsOnUpdatedLesson()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void UpdateLessonUserAchievementsOnCompletedLesson()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void UpdateChapterUserAchievementsOnCompletedChapter()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void UpdateCourseUserAchievementsOnCompletedCourse()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion
    
    
    private static LessonProgress CreateLessonProgress(Lesson lesson, User user, DateTime startTime, DateTime? completionTime) 
        => new()
        {
            Id = LessonProgressId,
            LessonId = lesson.Id,
            Lesson = lesson,
            UserUsername = user.Username,
            User = user,
            StartTime = startTime,
            CompletionTime = completionTime
        };
}