using APIInjDependencias.Interfaces;
using APIInjDependencias.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITesteA, TesteA>();
builder.Services.AddTransient<ITesteB, TesteB>();
builder.Services.AddScoped<TesteC>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();