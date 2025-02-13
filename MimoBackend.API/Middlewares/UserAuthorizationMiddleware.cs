using Microsoft.AspNetCore.WebUtilities;
using MimoBackend.API.Persistence.Caches;

namespace MimoBackend.API.Middlewares;

public class UserAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenCache cache;

    public UserAuthorizationMiddleware(RequestDelegate next, ITokenCache cache)
    {
        _next = next;
        this.cache = cache;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Query.TryGetValue("token", out var token) &&
            cache.IsTokenValid(token!))
        {
            var newQueryParams = QueryHelpers.ParseQuery(context.Request.QueryString.Value);
            newQueryParams["user"] = cache.GetUsername(token);
            
            var newQueryString = QueryHelpers.AddQueryString("", newQueryParams);
            context.Request.QueryString = new QueryString(newQueryString);

            // Call the next middleware/controller
            await _next(context);   
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing valid token");
        }
    }
}