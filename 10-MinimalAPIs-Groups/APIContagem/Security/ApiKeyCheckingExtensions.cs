using APIContagem.Models;

namespace APIContagem.Security;

public static class ApiKeyCheckingExtensions
{
    public static RouteGroupBuilder AddApiKeyCheckingEndpointFilter(
        this RouteGroupBuilder builder, WebApplication webApp)
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var configurations = (ApiKeyCheckingConfigurations)context.HttpContext
                .RequestServices.GetService(typeof(ApiKeyCheckingConfigurations))!;

            webApp.Logger.LogInformation("Verificando o uso de chave de acesso para a API...");
            bool canAccess = true;
            if (configurations is not null &&
                !String.IsNullOrWhiteSpace(configurations.HeaderName) &&
                !String.IsNullOrWhiteSpace(configurations.ApiKey))
                canAccess = context.HttpContext.Request.Headers.TryGetValue(configurations.HeaderName, out var valueApiKeyHeader) &&
                    configurations.ApiKey == (string)valueApiKeyHeader!;

            if (canAccess)
            {
                webApp.Logger.LogInformation("Acesso liberado ao recurso...");
                var result = await next(context);
                webApp.Logger.LogInformation($"Resultado processado... | {typeof(ResultadoContador)}");
                return result;
            }
            else
            {
                webApp.Logger.LogError(
                    $"API Key invalida para este recurso!");
                return Results.Unauthorized();
            }
        });
        return builder;
    }
}