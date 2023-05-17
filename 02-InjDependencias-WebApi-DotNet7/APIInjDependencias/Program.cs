using APIInjDependencias.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(
    new InfoInicializacao()
    {
        Horario = $"{DateTime.Now:HH:mm:ss}"
    });

// O trecho a seguir quando ativo desabilita o mecanismo de inferencia
// para injecao de dependencias em Controllers
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
     options.DisableImplicitFromServicesParameters = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();