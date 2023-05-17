using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Groffe.AspNetCore.ApiKeyChecking;

public class ApiKeyValidator
{
    private readonly RequestDelegate _next;

    public ApiKeyValidator(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var configurations = (ApiKeyCheckingConfigurations)httpContext
            .RequestServices.GetService(typeof(ApiKeyCheckingConfigurations))!;
        var logger = (ILogger<ApiKeyValidator>)httpContext
            .RequestServices.GetService(typeof(ILogger<ApiKeyValidator>))!;

        bool canAccess = true;
        if (configurations is not null &&
            !String.IsNullOrWhiteSpace(configurations.HeaderName) &&
            !String.IsNullOrWhiteSpace(configurations.ApiKey))
            canAccess = httpContext.Request.Headers.TryGetValue(configurations.HeaderName, out var valueApiKeyHeader) &&
                configurations.ApiKey == (string)valueApiKeyHeader;

        if (canAccess)
        {
            logger.LogInformation("Acesso liberado ao recurso...");
            await _next(httpContext);
        }
        else
        {
            logger.LogError(
                $"API Key invalida para este recurso!");
            httpContext.Response.StatusCode = 403;
        }
    }
}