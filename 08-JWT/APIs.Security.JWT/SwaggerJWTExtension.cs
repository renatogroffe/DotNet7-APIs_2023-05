using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace APIs.Security.JWT;

public static class SwaggerJWTExtension
{
    public static IServiceCollection AddSwaggerGenJwt(
        this IServiceCollection services,
        string versionApi, OpenApiInfo apiInfo)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(versionApi, apiInfo);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
                    "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
                    "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}