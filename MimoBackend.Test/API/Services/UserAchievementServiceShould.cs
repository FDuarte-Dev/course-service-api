using FluentAssertions;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class UserAchievementServiceShould
{
    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<IAchievementService> _achievementService = new();
    private readonly Mock<IUserAchievementRepository> _userAchievementRepository = new();

    private const string Username = "user1";
    
    private readonly User _user = new ()
    {
        Username = "usr",
        Password = "pwd"
    };

    private readonly UserAchievementService _service;

    public UserAchievementServiceShould()
    {
        _userService.Setup(x => x.GetUserBy(Username))
            .Returns(_user);
        
        _service = new UserAchievementService(
            _userService.Object,
            _achievementService.Object,
            _userAchievementRepository.Object);
    }

    #region GetUserAchievements

    [Fact]
    public void ReturnEmptyListIfMissingUser()
    {
        // Arrange
        _userService.Setup(x => x.GetUserBy(Username))
            .Returns(NotFoundUser.GetNotFoundUser);
        
        // Act
        var result = _service.GetUserAchievements(Username);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void ReturnUserAchievements()
    {
        // Arrange
        IEnumerable<UserAchievement> userAchievements = new List<UserAchievement> { new (){Id = 1} };
        _userAchievementRepository.Setup(x => x.GetUserAchievements(_user))
            .Returns(userAchievements);
        
        // Act
        var result = _service.GetUserAchievements(Username).ToList();
        
        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
    }
    
    [Fact]
    public void ReturnEmptyListIfUserHasNoAchievements()
    {
        // Arrange
        IEnumerable<UserAchievement> userAchievements = new List<UserAchievement>();
        _userAchievementRepository.Setup(x => x.GetUserAchievements(_user))
            .Returns(userAchievements);
        
        // Act
        var result = _service.GetUserAchievements(Username).ToList();
        
        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetCompletedUserAchievements

    [Fact]
    public void ReturnCompletedUserAchievements()
    {
        // Arrange
        IEnumerable<UserAchievement> userAchievements = new List<UserAchievement> { new (){Id = 1, Completed = true} };
        _userAchievementRepository.Setup(x => x.GetCompletedUserAchievements(_user))
            .Returns(userAchievements);
        
        // Act
        var result = _service.GetCompletedUserAchievements(Username).ToList();
        
        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
        result.First().Completed.Should().BeTrue();
    }
    
    [Fact]
    public void ReturnSuccessfulEmptyListIfUserHasNoCompletedAchievements()
    {
        // Arrange
        IEnumerable<UserAchievement> userAchievements = new List<UserAchievement> ();
        _userAchievementRepository.Setup(x => x.GetCompletedUserAchievements(_user))
            .Returns(userAchievements);
        
        // Act
        var result = _service.GetCompletedUserAchievements(Username).ToList();
        
        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetUserAchievementsInProgress

    [Fact]
    public void ReturnOngoingUserAchievements()
    {
        // Arrange
        IEnumerable<UserAchievement> userAchievements = new List<UserAchievement> { new (){Id = 1, Completed = false} };
        _userAchievementRepository.Setup(x => x.GetUserAchievementsInProgress(_user))
            .Returns(userAchievements);
        
        // Act
        var result = _service.GetOngoingUserAchievements(Username).ToList();
        
        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
        result.First().Completed.Should().BeFalse();
    }
    
    [Fact]
    public void ReturnSuccessfulEmptyListIfUserHasNoOngoingAchievements()
    {
        // Arrange
        IEnumerable<UserAchievement> userAchievements = new List<UserAchievement> ();
        _userAchievementRepository.Setup(x => x.GetUserAchievementsInProgress(_user))
            .Returns(userAchievements);
        
        // Act
        var result = _service.GetOngoingUserAchievements(Username).ToList();
        
        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region UpdateUserAchievement

    #region Lesson

    [Fact]
    public void UpdateUserAchievementWithLessonAndUser()
    {
        // Arrange
        var lessonAchievement = new Achievement(){ Type = AchievementType.CompletedLessons};
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedLessons))
            .Returns(new List<Achievement>(){lessonAchievement});
        
        UserAchievement userAchievement = new (){Id = 1, Completed = false};
        _userAchievementRepository.Setup(x => x.GetUserAchievementByAchievementUserAndCompletion(lessonAchievement, _user, false))
            .Returns(userAchievement);
        
        // Act
        _service.UpdateLessonUserAchievement(_user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(userAchievement), Times.Once);
    }
    
    [Fact]
    public void NotUpdateUserAchievementIfAchievementNotFound()
    {
        // Arrange
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedLessons))
            .Returns(new List<Achievement>());
        
        // Act
        _service.UpdateLessonUserAchievement(_user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(It.IsAny<UserAchievement>()), Times.Never);
    }

    [Fact]
    public void CreateUserAchievementBeforeUpdatingIfUserAchievementNotFound()
    {
        // Arrange
        var lessonAchievement = new Achievement(){ Type = AchievementType.CompletedLessons};
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedLessons))
            .Returns(new List<Achievement>(){lessonAchievement});
        
        _userAchievementRepository.Setup(x => x.GetUserAchievementByAchievementUserAndCompletion(lessonAchievement, _user, false))
            .Returns((UserAchievement?)null);
        
        // Act
        _service.UpdateLessonUserAchievement(_user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.CreateUserAchievement(It.IsAny<UserAchievement>()), Times.Once);
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(It.IsAny<UserAchievement>()), Times.Once);
    }

    #endregion

    #region Chapter

    [Fact]
    public void UpdateUserAchievementWithChapterAndUser()
    {
        // Arrange
        var achievement = new Achievement(){ Type = AchievementType.CompletedChapters};
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedChapters))
            .Returns(new List<Achievement>(){achievement});
        
        UserAchievement userAchievement = new (){Id = 1, Completed = false};
        _userAchievementRepository.Setup(x => x.GetUserAchievementByAchievementUserAndCompletion(achievement, _user, false))
            .Returns(userAchievement);
        
        // Act
        _service.UpdateChapterUserAchievement(_user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(userAchievement), Times.Once);
    }
    
    [Fact]
    public void NotUpdateUserAchievementIfChapterNotFound()
    {
        // Arrange
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedChapters))
            .Returns(new List<Achievement>());
        
        // Act
        _service.UpdateChapterUserAchievement(_user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(It.IsAny<UserAchievement>()), Times.Never);
    }
    
    [Fact]
    public void NotUpdateChapterUserAchievementIfUserNotFound()
    {
        // Arrange
        var achievement = new Achievement(){ Type = AchievementType.CompletedChapters};
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedChapters))
            .Returns(new List<Achievement>(){achievement});
        
        _userAchievementRepository.Setup(x => x.GetUserAchievementByAchievementUserAndCompletion(achievement, _user, false))
            .Returns((UserAchievement?)null);
        
        // Act
        _service.UpdateChapterUserAchievement(_user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.CreateUserAchievement(It.IsAny<UserAchievement>()), Times.Once);
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(It.IsAny<UserAchievement>()), Times.Once);
    }

    #endregion

    #region Course

    [Fact]
    public void UpdateUserAchievementWithCourseAndUser()
    {
        // Arrange
        var course = new Course { Name = "TEST" };
        var achievement = new Achievement(){ Type = AchievementType.CompletedCourses, Name = "TEST Course"};
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedCourses))
            .Returns(new List<Achievement>(){achievement});
        
        UserAchievement userAchievement = new (){Id = 1, Completed = false};
        _userAchievementRepository.Setup(x => x.GetUserAchievementByAchievementUserAndCompletion(achievement, _user, false))
            .Returns(userAchievement);
        
        // Act
        _service.UpdateCourseUserAchievement(course, _user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(userAchievement), Times.Once);

    }
    
    [Fact]
    public void NotUpdateUserAchievementIfCourseNotFound()
    {
        // Arrange
        var course = new Course { Name = "TEST" };
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedCourses))
            .Returns(new List<Achievement>());
        
        // Act
        _service.UpdateCourseUserAchievement(course, _user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(It.IsAny<UserAchievement>()), Times.Never);
    }
    
    [Fact]
    public void NotUpdateCourseUserAchievementIfUserNotFound()
    {
        // Arrange
        var course = new Course { Name = "TEST" };
        var achievement = new Achievement(){ Type = AchievementType.CompletedCourses, Name = "TEST Course"};
        _achievementService.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedCourses))
            .Returns(new List<Achievement>(){achievement});
        
        _userAchievementRepository.Setup(x => x.GetUserAchievementByAchievementUserAndCompletion(achievement, _user, false))
            .Returns((UserAchievement?)null);
        
        // Act
        _service.UpdateCourseUserAchievement(course, _user);
        
        // Assert
        _userAchievementRepository.Verify(x => x.CreateUserAchievement(It.IsAny<UserAchievement>()), Times.Once);
        _userAchievementRepository.Verify(x => x.UpdateUserAchievement(It.IsAny<UserAchievement>()), Times.Once);

    }

    #endregion

    #endregion
}