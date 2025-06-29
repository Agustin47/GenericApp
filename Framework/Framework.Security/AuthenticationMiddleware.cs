using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Framework.Security;

public class AuthenticationMiddleware(ISecurityService security, ILogger<AuthenticationMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        
        var metadata = context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>();

        if (metadata != null)
        {
            logger.LogInformation("Skipping introspection");
            await next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].ToString() ?? string.Empty;
        token = token.Replace("Bearer ", string.Empty);
        var userContextResult = security.GetUserContext(token);

        if (userContextResult.IsFailed ||
            security.IntrospectToken(token, userContextResult.Value.Username).IsFailed)
        {
            await ReturnUnauthorize(context);
            return;
        }
        
        await next(context);
    }

    private async Task ReturnUnauthorize(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Do not have permission to access this resource");
    }
}