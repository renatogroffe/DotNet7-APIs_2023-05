using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace APIs.Security.JWT;

public static class JwtSecurityExtension
{
    public static IServiceCollection AddJwtSecurity(
        this IServiceCollection services,
        TokenConfigurations tokenConfigurations)
    {
        // Ativando a utilização do ASP.NET Identity, a fim de
        // permitir a recuperação de seus objetos via injeção de
        // dependências
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApiSecurityDbContext>()
            .AddDefaultTokenProviders();

        // Configurando a dependência para a classe de validação
        // de credenciais e geração de tokens
        services.AddScoped<AccessManager>();
        services.AddScoped<JwtSecurityExtensionEvents>();

        var signingConfigurations = new SigningConfigurations(
            tokenConfigurations.SecretJwtKey!);
        services.AddSingleton(signingConfigurations);

        services.AddSingleton(tokenConfigurations);

        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            var paramsValidation = bearerOptions.TokenValidationParameters;
            paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            paramsValidation.ValidAudience = tokenConfigurations.Audience;
            paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

            // Valida a assinatura de um token recebido
            paramsValidation.ValidateIssuerSigningKey = true;

            // Verifica se um token recebido ainda é válido
            paramsValidation.ValidateLifetime = true;

            // Tempo de tolerância para a expiração de um token (utilizado
            // caso haja problemas de sincronismo de horário entre diferentes
            // computadores envolvidos no processo de comunicação)
            paramsValidation.ClockSkew = TimeSpan.Zero;

            bearerOptions.EventsType = typeof(JwtSecurityExtensionEvents);
        });

        // Ativa o uso do token como forma de autorizar o acesso
        // a recursos deste projeto
        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser().Build());
        });

        return services;
    }
}