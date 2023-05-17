using Microsoft.OpenApi.Models;
using APIContagem;
using APIContagem.Models;
using APIContagem.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "APIContagem",
            Description = "Exemplo de implementação de Minimal API para Contagem de acessos", 
            Version = "v1",
            Contact = new OpenApiContact()
            {
                Name = "Renato Groffe",
                Url = new Uri("https://github.com/renatogroffe"),
            },
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("http://opensource.org/licenses/MIT"),
            }
        });
});

builder.Services.AddSingleton<Contador>();
builder.Services.AddSingleton(new ApiKeyCheckingConfigurations()
{
    HeaderName = builder.Configuration["ApiKeyChecking:Header"],
    ApiKey = builder.Configuration["ApiKeyChecking:Key"]
});

builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIContagem v1");
});

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/contador", (Contador contador) =>
{
    int valorAtualContador;
    lock (contador)
    {
        contador.Incrementar();
        valorAtualContador = contador.ValorAtual;
    }
    app.Logger.LogInformation($"Contador - Valor atual: {valorAtualContador}");    

    return Results.Ok(new ResultadoContador()
    {
        ValorAtual = contador.ValorAtual,
        Local = contador.Local,
        Kernel = contador.Kernel,
        Framework = contador.Framework,
        Mensagem = app.Configuration["Saudacao"]
    });
})
.Produces<ResultadoContador>()
.AddEndpointFilter(async (context, next) =>
{
    var configurations = (ApiKeyCheckingConfigurations)context.HttpContext
        .RequestServices.GetService(typeof(ApiKeyCheckingConfigurations))!;

    app.Logger.LogInformation("Verificando o uso de chave de acesso para a API...");
    bool canAccess = true;
    if (configurations is not null &&
        !String.IsNullOrWhiteSpace(configurations.HeaderName) &&
        !String.IsNullOrWhiteSpace(configurations.ApiKey))
        canAccess = context.HttpContext.Request.Headers.TryGetValue(configurations.HeaderName, out var valueApiKeyHeader) &&
            configurations.ApiKey == (string)valueApiKeyHeader!;

    if (canAccess)
    {
        app.Logger.LogInformation("Acesso liberado ao recurso...");
        var result = await next(context);
        app.Logger.LogInformation($"Resultado processado... | {typeof(ResultadoContador)}");
        return result;
    }
    else
    {
        app.Logger.LogError(
            $"API Key invalida para este recurso!");
        return Results.Unauthorized();
    }
})
.Produces(StatusCodes.Status401Unauthorized);

app.Run();