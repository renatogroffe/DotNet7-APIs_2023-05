using Microsoft.OpenApi.Models;
using APIInjDependencias.Interfaces;
using APIInjDependencias.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITesteA, TesteA>();
builder.Services.AddTransient<ITesteB, TesteB>();
builder.Services.AddScoped<TesteC>();
builder.Services.AddTransient<TesteInjecao>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "APIInjDependencias",
            Description = "Exemplo de utilizacao de injecao de dependencias com Minimal APIs",
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

var app = builder.Build();

app.MapGet("/", (TesteInjecao objTesteInjecao, ITesteA testeA,
    ITesteB testeB, TesteC testeC) =>
{
    var resultado = objTesteInjecao.RetornarValoresInjecao(testeA, testeB, testeC);
    app.Logger.LogInformation(resultado.ValoresA!.ToString());
    app.Logger.LogInformation(resultado.ValoresB!.ToString());
    app.Logger.LogInformation(resultado.ValoresC!.ToString());
    return resultado;
}).Produces<ResultadoInjecao>();

app.Run();