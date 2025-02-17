using FluentAssertions;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class UserServiceShould : BaseServiceTest
{
    private readonly Mock<IUserRepository> _userRepository = new();
    
    private readonly UserService _service;
    
    public UserServiceShould()
    {
        _service = new UserService(_userRepository.Object);
    }

    #region GetUserBy

    [Fact]
    public void ReturnUserIfItExists()
    {
        // Arrange
        var expectedUser = new User(){Username = Username};
        
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns(expectedUser);
        
        // Act
        var result = _service.GetUserBy(Username);
        
        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(Username);
    }
    
    [Fact]
    public void ReturnNotFoundUserIfItDoesNotExist()
    {
        // Arrange
        var expectedUser = NotFoundUser.GetNotFoundUser();
        
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns((User?)null);
        
        // Act
        var result = _service.GetUserBy(Username);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedUser);
    }

    #endregion
}