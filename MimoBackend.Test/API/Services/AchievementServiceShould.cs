using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class AchievementServiceShould : BaseServiceTest
{
    private readonly Mock<IAchievementRepository> _achievementRepository = new();

    private readonly AchievementService _service;

    public AchievementServiceShould()
    {
        _service = new AchievementService(_achievementRepository.Object);
    }

    #region GetAchievements

    [Fact]
    public void ReturnEmptyListOnNoLessonsFound()
    {
        // Arrange
        _achievementRepository.Setup(x => x.GetAchievements())
            .Returns([]);
        
        // Act
        var result = _service.GetAchievements(Username);

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void ReturnSuccessLessonsList()
    {
        // Arrange
        var achievements = new List<Achievement> { new() { Id = 1 } };
        _achievementRepository.Setup(x => x.GetAchievements())
            .Returns(achievements);
        
        // Act
        var result = _service.GetAchievements(Username);

        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
    }

    #endregion
}