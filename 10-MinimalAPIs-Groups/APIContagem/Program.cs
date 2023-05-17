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

var contagemApi = app.MapGroup("/contador").AddApiKeyCheckingEndpointFilter(webApp: app);

contagemApi.MapGet("/incrementar", (Contador contador) =>
{
    int valorAtualContador;
    lock (contador)
    {
        contador.Incrementar();
        valorAtualContador = contador.ValorAtual;
    }
    app.Logger.LogInformation($"Contador - Valor atual com Incremento: {valorAtualContador}");    

    return Results.Ok(new ResultadoContador()
    {
        ValorAtual = contador.ValorAtual,
        Local = contador.Local,
        Kernel = contador.Kernel,
        Framework = contador.Framework,
        Mensagem = $"{app.Configuration["Saudacao"]} | Incrementando"
    });
}).Produces<ResultadoContador>().Produces(StatusCodes.Status401Unauthorized);

contagemApi.MapGet("/decrementar", (Contador contador) =>
{
    int valorAtualContador;
    lock (contador)
    {
        contador.Decrementar();
        valorAtualContador = contador.ValorAtual;
    }
    app.Logger.LogInformation($"Contador - Valor atual com Decremento: {valorAtualContador}");    

    return Results.Ok(new ResultadoContador()
    {
        ValorAtual = contador.ValorAtual,
        Local = contador.Local,
        Kernel = contador.Kernel,
        Framework = contador.Framework,
        Mensagem = $"{app.Configuration["Saudacao"]} | Decrementando"
    });
}).Produces<ResultadoContador>().Produces(StatusCodes.Status401Unauthorized);

app.Run();