using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using APIs.Security.JWT;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
// Configurando o uso do Swagger para prever tokens JWT
builder.Services.AddSwaggerGenJwt("v1",
    new OpenApiInfo
    {
        Title = "APIContagem",
        Description = "Exemplo de implementação de uso de JWT em uma API de contagem de acessos",
        Version = "v1"
    });

// Configurando o uso da classe de contexto para
// acesso às tabelas do ASP.NET Identity Core
builder.Services.AddDbContext<ApiSecurityDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDatabase"));

var tokenConfigurations = new TokenConfigurations();
new ConfigureFromConfigurationOptions<TokenConfigurations>(
    builder.Configuration.GetSection("TokenConfigurations"))
        .Configure(tokenConfigurations);

// Aciona a extensão que irá configurar o uso de
// autenticação e autorização via tokens
builder.Services.AddJwtSecurity(tokenConfigurations);

// Acionar caso seja necessário criar usuários para testes
builder.Services.AddScoped<IdentityInitializer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

// Criação de estruturas, usuários e permissões
// na base do ASP.NET Identity Core (caso ainda não
// existam)
using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IdentityInitializer>().Initialize();

app.UseAuthorization();

app.MapControllers();

app.Run();