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
        Order = 1,
        ChapterId = 1
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

        _chapterService.Setup(x => x.UserCompletedChapter(It.IsAny<Chapter>(), It.IsAny<User>()))
            .Returns(false);
        _courseService.Setup(x => x.UserCompletedCourse(It.IsAny<Course>(), It.IsAny<User>()))
            .Returns(false);
        
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
        result.Should().NotBeNull();
        result.Id.Should().BePositive();
    }

    [Fact]
    public void ReturnNotFoundIfMissingLesson()
    {
        // Arrange
        _lessonService.Setup(x => x.GetLessonBy(_lesson.Id))
            .Returns(NotFoundLesson.GetNotFoundLesson());
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        result.Should().BeAssignableTo(typeof(NotFoundLessonProgress));
    }
    
    [Fact]
    public void ReturnNotFoundIfMissingUser()
    {
        // Arrange
        _userService.Setup(x => x.GetUserBy(Username))
            .Returns(NotFoundUser.GetNotFoundUser);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        result.Should().BeAssignableTo(typeof(NotFoundLessonProgress));
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
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Fact]
    public void CreateNewLessonProgressWhenReStartingCompletedLesson()
    {
        // Arrange
        var lessonProgress = 
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.FindByLessonUserAndCompletion(_lesson, _user, true))
            .Returns(NotFoundLessonProgress.GetNotFoundLessonProgress);
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(lessonProgress);
        
        // Act
        var result = _service.StartLesson(_lesson.Id, DateTime.Today, Username);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
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
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Fact]
    public void ReturnNotFoundOnNonexistentLessonProgressOnCompleteLesson()
    {
        // Arrange
        _lpRepository.Setup(x => x.FindByLessonUserAndCompletion(_lesson, _user, false))
            .Returns(NotFoundLessonProgress.GetNotFoundLessonProgress);
        
        // Act
        var result = _service.CompleteLesson(_lesson.Id, _lessonUpdate.CompletionTime, Username);
        
        // Assert
        result.Should().BeAssignableTo(typeof(NotFoundLessonProgress));

    }

    #endregion

    #region UserAchievementUpdates

    [Fact]
    public void UpdateLessonUserAchievementsOnUpdatedLesson()
    {
        // Arrange
        var expectedLessonUpdate =
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(expectedLessonUpdate);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BePositive();
        _userAchievementService.Verify(x => x.UpdateLessonUserAchievement(_user), Times.Once);
    }
    
    [Fact]
    public void UpdateLessonUserAchievementsOnCompletedLesson()
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
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _userAchievementService.Verify(x => x.UpdateLessonUserAchievement(_user), Times.Once);
    }
    
    [Fact]
    public void UpdateChapterUserAchievementsOnCompletedChapter()
    {
        // Arrange
        var chapter = new Chapter();
        var expectedLessonUpdate =
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(expectedLessonUpdate);
        _chapterService.Setup(x => x.GetChapterBy(_lesson.ChapterId))
            .Returns(chapter);
        _chapterService.Setup(x => x.UserCompletedChapter(chapter, _user))
            .Returns(true);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BePositive();
        _userAchievementService.Verify(x => x.UpdateLessonUserAchievement(_user), Times.Once);
        _userAchievementService.Verify(x => x.UpdateChapterUserAchievement(_user), Times.Once);
    }
    
    [Fact]
    public void UpdateCourseUserAchievementsOnCompletedCourse()
    {
        // Arrange
        var course = new Course();
        var chapter = new Chapter()
        {
            CourseId = 1
        };
        var expectedLessonUpdate =
            CreateLessonProgress(_lesson, _user, _lessonUpdate.StartTime, _lessonUpdate.CompletionTime);
        _lpRepository.Setup(x => x.AddLessonProgress(It.IsAny<LessonProgress>()))
            .Returns(expectedLessonUpdate);
        
        _chapterService.Setup(x => x.GetChapterBy(_lesson.ChapterId))
            .Returns(chapter);
        _chapterService.Setup(x => x.UserCompletedChapter(chapter, _user))
            .Returns(true);
        
        _courseService.Setup(x => x.GetCourseBy(chapter.CourseId))
            .Returns(course);
        _courseService.Setup(x => x.UserCompletedCourse(course, _user))
            .Returns(true);
        
        // Act
        var result = _service.UpdateLesson(_lesson.Id, _lessonUpdate, Username);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BePositive();
        _userAchievementService.Verify(x => x.UpdateLessonUserAchievement(_user), Times.Once);
        _userAchievementService.Verify(x => x.UpdateChapterUserAchievement(_user), Times.Once);
        _userAchievementService.Verify(x => x.UpdateCourseUserAchievement(course, _user), Times.Once);
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