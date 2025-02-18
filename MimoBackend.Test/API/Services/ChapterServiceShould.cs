using FluentAssertions;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class ChapterServiceShould : BaseServiceTest
{
    private readonly Mock<ILessonService> _lessonService = new();
    private readonly Mock<ILessonProgressService> _lessonProgressService = new();
    private readonly Mock<IChapterRepository> _chapterRepository = new();
    
    private readonly ChapterService _service;
    
    public ChapterServiceShould()
    {
        _service = new ChapterService(
            _lessonService.Object,
            _lessonProgressService.Object,
            _chapterRepository.Object);
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
        var chapter = new Chapter {Id = 1};
        var lesson = new Lesson {Id = 1};
        var lessons = new List<Lesson> { lesson };
        _lessonService.Setup(x => x.GetChapterLessons(1))
            .Returns(lessons);
        _lessonProgressService.Setup(x => x.UserCompletedLesson(lesson, User))
            .Returns(true);
        
        // Act
        var result = _service.UserCompletedChapter(chapter, User);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void ReturnFalseIfUserDidNotCompletedAllLessonsInChapter()
    {
        // Arrange
        var chapter = new Chapter {Id = 1};
        var lesson = new Lesson {Id = 1};
        var lessons = new List<Lesson> { lesson };
        _lessonService.Setup(x => x.GetChapterLessons(1))
            .Returns(lessons);
        _lessonProgressService.Setup(x => x.UserCompletedLesson(lesson, User))
            .Returns(false);
        
        // Act
        var result = _service.UserCompletedChapter(chapter, User);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void ReturnFalseChapterHasNoLessons()
    {
        // Arrange
        var chapter = new Chapter {Id = 1};
        _lessonService.Setup(x => x.GetChapterLessons(1))
            .Returns(Enumerable.Empty<Lesson>());
        
        // Act
        var result = _service.UserCompletedChapter(chapter, User);
        
        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region GetCourseChapters

    [Fact]
    public void ReturnEmptyListIfNoChaptersAreFoundForTheCourse()
    {
        // Arrange
        _chapterRepository.Setup(x => x.GetCourseChapters(1))
            .Returns(Enumerable.Empty<Chapter>());
        
        // Act
        var result = _service.GetCourseChapters(1);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void ReturnListWithCourseChapters()
    {
        // Arrange
        IEnumerable<Chapter> chapters = new List<Chapter>(){new(){Id = 1}};
        _chapterRepository.Setup(x => x.GetCourseChapters(1))
            .Returns(chapters);
        
        // Act
        var result = _service.GetCourseChapters(1).ToList();
        
        // Assert
        result.Should().NotBeEmpty();
        result.First().Id.Should().Be(1);
    }

    #endregion
}