using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MimoBackend.API.Models;
using MimoBackend.API.Models.DTOs;
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

    [Fact]
    public async Task ReturnValidTokenOnSuccessfulLogin()
    {
        // Arrange
        var credentials = new Credentials(Username, Password);
        _userRepository.Setup(x => x.GetUser(Username))
            .ReturnsAsync(new UserDto()
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
        var result = await _service.Authorize(credentials);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(200);
        (result as ContentResult)!.Content.Should().Be("{\"Token\":\"TOKEN\"}");
    }

    [Fact]
    public async Task ReturnNotFoundOnMissingUser()
    {
        // Arrange
        var credentials = new Credentials(Username, Password);
        _userRepository.Setup(x => x.GetUser(Username))
            .ReturnsAsync((UserDto?)null);
        
        // Act
        var result = await _service.Authorize(credentials);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(404);
        (result as ContentResult)!.Content.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ReturnForbiddenOnIncorrectPassword()
    {
        // Arrange
        const string otherPassword = "other-pwd";
        var credentials = new Credentials(Username, Password);
        _userRepository.Setup(x => x.GetUser(Username))
            .ReturnsAsync(new UserDto()
            {
                Username = Username,
                Password = otherPassword
            });
        
        // Act
        var result = await _service.Authorize(credentials);
        
        // Assert
        (result as ContentResult)!.StatusCode.Should().Be(403);
        (result as ContentResult)!.Content.Should().BeNullOrWhiteSpace();
    }
}