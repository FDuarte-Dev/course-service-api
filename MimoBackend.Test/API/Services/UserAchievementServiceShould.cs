using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class UserAchievementServiceShould
{
    private readonly Mock<IAchievementRepository> _achievementRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
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
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns(_user);
        
        _service = new UserAchievementService(
            _userRepository.Object,
            _achievementRepository.Object,
            _userAchievementRepository.Object);
    }

    #region GetUserAchievements

    [Fact]
    public void ReturnNotFoundIfMissingUser()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnNotFoundIfNoAchievementsFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnUserAchievements()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnSuccessfulEmptyListIfUserHasNoAchievements()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion

    #region GetCompletedUserAchievements

    [Fact]
    public void ReturnCompletedUserAchievements()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnSuccessfulEmptyListIfUserHasNoCompletedAchievements()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion

    #region GetUserAchievementsInProgress

    [Fact]
    public void ReturnOngoingUserAchievements()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnSuccessfulEmptyListIfUserHasNoOngoingAchievements()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion

    #region UpdateUserAchievement

    #region Lesson

    [Fact]
    public void UpdateUserAchievementWithLessonAndUser()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void NotUpdateUserAchievementIfLessonNotFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    [Fact]
    public void NotUpdateLessonUserAchievementIfUserNotFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion

    #region Chapter

    [Fact]
    public void UpdateUserAchievementWithChapterAndUser()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void NotUpdateUserAchievementIfChapterNotFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void NotUpdateChapterUserAchievementIfUserNotFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion

    #region Course

    [Fact]
    public void UpdateUserAchievementWithCourseAndUser()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void NotUpdateUserAchievementIfCourseNotFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void NotUpdateCourseUserAchievementIfUserNotFound()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion

    #endregion
}