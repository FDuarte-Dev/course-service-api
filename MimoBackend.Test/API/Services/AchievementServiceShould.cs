using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class AchievementServiceShould
{
    private readonly Mock<IAchievementRepository> _achievementRepository = new();

    private const string Username = "user1";

    private readonly AchievementService _service;

    public AchievementServiceShould()
    {
        _service = new AchievementService(_achievementRepository.Object);
    }

    [Fact]
    public void ReturnSuccessWithEmptyListOnNoLessonsFound()
    {
        // Arrange
        _achievementRepository.Setup(x => x.GetAchievements())
            .Returns([]);
        
        // Act
        var result = _service.GetAchievements(Username);

        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("[]");
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
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("\"Id\":1");
    }
}