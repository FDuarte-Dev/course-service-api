using FluentAssertions;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class CourseServiceShould : BaseServiceTest
{
    private readonly Mock<ICourseRepository> _courseRepository = new();
    
    private readonly CourseService _service;
    
    public CourseServiceShould()
    {
        _service = new CourseService(_courseRepository.Object);
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
        // Act
        // Assert
        Assert.Fail();
    }
    
    [Fact]
    public void ReturnFalseIfUserDidNotCompletedAllChaptersInCourse()
    {
        // Arrange
        // Act
        // Assert
        Assert.Fail();
    }

    #endregion
}