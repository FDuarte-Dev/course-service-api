using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
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
    public void ReturnEmptyListOnNoAchievementsFound()
    {
        // Arrange
        _achievementRepository.Setup(x => x.GetAchievements())
            .Returns([]);
        
        // Act
        var result = _service.GetAchievements();

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void ReturnSuccessAchievementsList()
    {
        // Arrange
        var achievements = new List<Achievement> { new() { Id = 1 } };
        _achievementRepository.Setup(x => x.GetAchievements())
            .Returns(achievements);
        
        // Act
        var result = _service.GetAchievements();

        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
    }

    #endregion

    #region GetAchievementOfType

    [Fact]
    public void ReturnListOfAchievementsThatMatchTheGivenType()
    {
        // Arrange
        var achievements = new List<Achievement> { new() { Id = 1, Type = AchievementType.CompletedCourses} };
        _achievementRepository.Setup(x => x.GetAchievementsOfType(AchievementType.CompletedCourses))
            .Returns(achievements);
        
        // Act
        var result = _service.GetAchievementsOfType(AchievementType.CompletedCourses).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
        result.First().Type.Should().Be(AchievementType.CompletedCourses);
    }
    
    [Fact]
    public void ReturnEmptyListIfNoAchievementMatchesTheGivenType()
    {
        // Arrange
        var achievements = new List<Achievement> ();
        _achievementRepository.Setup(x => x.GetAchievements())
            .Returns(achievements);
        
        // Act
        var result = _service.GetAchievementsOfType(AchievementType.CompletedCourses).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion
}