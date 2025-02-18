using FluentAssertions;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class ChapterServiceShould : BaseServiceTest
{
    private readonly Mock<IChapterRepository> _chapterRepository = new();
    
    private readonly ChapterService _service;
    
    public ChapterServiceShould()
    {
        _service = new ChapterService(_chapterRepository.Object);
    }
    
    #region GetChapterBy

    [Fact]
    public void ReturnChapterIfItExists()
    {
        // Arrange
        var expectedChapter = new Chapter(){Id = 1};
        
        _chapterRepository.Setup(x => x.GetChapterBy(1))
            .Returns(expectedChapter);
        
        // Act
        var result = _service.GetChapterBy(1);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Fact]
    public void ReturnNotFoundChapterIfItDoesNotExist()
    {
        // Arrange
        var expectedChapter = NotFoundChapter.GetNotFoundChapter();
        
        _chapterRepository.Setup(x => x.GetChapterBy(1))
            .Returns((Chapter?)null);
        
        // Act
        var result = _service.GetChapterBy(1);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedChapter);
    }

    #endregion

    #region UserCompletedChapter

    [Fact]
    public void ReturnTrueIfUserCompletedAllLessonsInChapter()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnFalseIfUserDidNotCompletedAllLessonsInChapter()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion
}