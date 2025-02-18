using FluentAssertions;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class CourseServiceShould : BaseServiceTest
{
    private readonly Mock<IChapterService> _chapterService = new();
    private readonly Mock<ICourseRepository> _courseRepository = new();
    
    private readonly CourseService _service;
    
    public CourseServiceShould()
    {
        _service = new CourseService(
            _chapterService.Object,
            _courseRepository.Object);
    }
    
    #region GetCourseBy

    [Fact]
    public void ReturnCourseIfItExists()
    {
        // Arrange
        var expectedChapter = new Course(){Id = 1};
        
        _courseRepository.Setup(x => x.GetCourseBy(1))
            .Returns(expectedChapter);
        
        // Act
        var result = _service.GetCourseBy(1);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Fact]
    public void ReturnNotFoundCourseIfItDoesNotExist()
    {
        // Arrange
        var expectedCourse = NotFoundCourse.GetNotFoundCourse();
        
        _courseRepository.Setup(x => x.GetCourseBy(1))
            .Returns((Course?)null);
        
        // Act
        var result = _service.GetCourseBy(1);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedCourse);
    }

    #endregion

    #region UserCompletedCourse
    
    [Fact]
    public void ReturnTrueIfUserCompletedAllChaptersInCourse()
    {
        // Arrange
        var course = new Course {Id = 1};
        var chapter = new Chapter();
        var chapters = new List<Chapter> { chapter };
        _chapterService.Setup(x => x.GetCourseChapters(course.Id))
            .Returns(chapters);
        _chapterService.Setup(x => x.UserCompletedChapter(chapter, User))
            .Returns(true);
        
        // Act
        var result = _service.UserCompletedCourse(course, User);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void ReturnFalseIfUserDidNotCompletedAllChaptersInCourse()
    {
        // Arrange
        var course = new Course {Id = 1};
        _chapterService.Setup(x => x.GetCourseChapters(course.Id))
            .Returns(Enumerable.Empty<Chapter>());
        
        // Act
        var result = _service.UserCompletedCourse(course, User);
        
        // Assert
        result.Should().BeTrue();
    }

    #endregion
}