using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Groffe.AspNetCore.ApiKeyChecking;

public static class ApiKeyCheckingExtensions
{
    public static IServiceCollection ConfigureApiKeyChecking(
        this IServiceCollection services,
        string headerNameChecking,
        string apiKey)
    {
        services.AddSingleton(new ApiKeyCheckingConfigurations()
        {
            HeaderName = headerNameChecking,
            ApiKey = apiKey
        });
        
        return services;
    }

    public static IApplicationBuilder UseApiKeyChecking(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiKeyValidator>();
    }
}