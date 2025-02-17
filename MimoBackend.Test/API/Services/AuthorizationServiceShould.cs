using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DatabaseObjects;
using MimoBackend.API.Persistence.Caches;
using MimoBackend.API.Repositories;
using MimoBackend.API.Services;
using Moq;

namespace MimoBackend.Test.API.Services;

public class AuthorizationServiceShould
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ITokenCache> _tokenCache = new();
    
    private const string Username = "usr";
    private const string Password = "pwd";
    private const string Token = "TOKEN";
    
    private readonly AuthorizationService _service;

    public AuthorizationServiceShould()
    {
        _service = new AuthorizationService(_userRepository.Object, _tokenCache.Object);
    }

    #region Authorize

    [Fact]
    public void ReturnValidTokenOnSuccessfulLogin()
    {
        // Arrange
        var credentials = new Credentials(Username, Password);
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns(new User()
            {
                Username = Username,
                Password = Password
            });
        _tokenCache.Setup(x => x.GenerateNewToken(credentials))
            .Returns(new AuthenticationToken()
            {
                Token = Token
            });
        
        // Act
        var result = _service.Authorize(credentials);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result as ContentResult)!.Content.Should().Contain("TOKEN");
    }

    [Fact]
    public void ReturnNotFoundOnMissingUser()
    {
        // Arrange
        var credentials = new Credentials(Username, Password);
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns((User?)null);
        
        // Act
        var result = _service.Authorize(credentials);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        (result as ContentResult)!.Content.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void ReturnForbiddenOnIncorrectPassword()
    {
        // Arrange
        const string otherPassword = "other-pwd";
        var credentials = new Credentials(Username, Password);
        _userRepository.Setup(x => x.GetUserBy(Username))
            .Returns(new User()
            {
                Username = Username,
                Password = otherPassword
            });
        
        // Act
        var result = _service.Authorize(credentials);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        (result as ContentResult)!.Content.Should().BeNullOrWhiteSpace();
    }

    #endregion
}