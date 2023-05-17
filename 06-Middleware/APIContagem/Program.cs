using Groffe.AspNetCore.ApiKeyChecking;

var builder = WebApplication.CreateBuilder(args);

var headerApiKeyChecking = builder.Configuration["ApiKeyChecking:Header"];
var valueApiKeyChecking = builder.Configuration["ApiKeyChecking:Key"];
bool useApiKeyChecking = (!String.IsNullOrWhiteSpace(headerApiKeyChecking) &&
    !String.IsNullOrWhiteSpace(valueApiKeyChecking));
if (useApiKeyChecking)
    builder.Services.ConfigureApiKeyChecking(headerApiKeyChecking!, valueApiKeyChecking!);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (useApiKeyChecking)
    app.Logger.LogInformation("Ativado o uso do middleware UseApiKeyChecking...");
    
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();