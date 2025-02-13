using FluentAssertions;
using MimoBackend.API.Models;
using MimoBackend.API.Persistence.Caches;

namespace MimoBackend.Test.API.Persistence.Caches;

public class TokenCacheShould
{
    private static readonly Credentials Credentials = new ("usr", "pwd");
    
    private TokenCache _cache;

    [Fact]
    public void GenerateNewTokenAndStoreInCache()
    {
        // Arrange
        _cache = new TestableTokenCache();
        
        // Act
        var token = _cache.GenerateNewToken(Credentials);
        
        // Assert
        token.Token.Should().NotBeNullOrEmpty();
        (_cache as TestableTokenCache)!.CacheSize.Should().Be(1);
    }

    [Fact]
    public void RemoveExpiredTokenFromCache()
    {
        // Arrange
        var expiredTimestamp = new DateTimeOffset(DateTime.MinValue).ToUnixTimeSeconds();
        var oldToken = new AuthenticationToken(){Token = "oldToken", Expires = expiredTimestamp};
        var testCache = new Dictionary<string, AuthenticationToken>()
        {
            {"oldToken", oldToken}
        };
        
        _cache = new TestableTokenCache();
        (_cache as TestableTokenCache)!.PopulateCache(testCache);
        
        // Act
        var result = _cache.IsTokenValid("oldToken");
        
        // Assert
        result.Should().BeFalse();
        (_cache as TestableTokenCache)!.CacheSize.Should().Be(0);
    }
    
    [Fact]
    public void ReturnOwnerUsernameFromToken()
    {
        // Arrange
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        const string username = "username";
        var token = new AuthenticationToken(){Token = "token", Expires = timestamp, Username = username};
        var testCache = new Dictionary<string, AuthenticationToken>()
        {
            {"token", token}
        };
        
        _cache = new TestableTokenCache();
        (_cache as TestableTokenCache)!.PopulateCache(testCache);
        
        // Act
        var result = _cache.GetUsername("token");
        
        // Assert
        result.Should().Be(username);
    }

    private class TestableTokenCache : TokenCache
    {
        public int CacheSize => Cache.Count;

        public void PopulateCache(Dictionary<string, AuthenticationToken> cache)
        {
            Cache = cache;
        }
    }
}