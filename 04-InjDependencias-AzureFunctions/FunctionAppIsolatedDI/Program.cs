using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FunctionAppIsolatedDI.Interfaces;
using FunctionAppIsolatedDI.Implementations;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((services) =>
    {
        services.AddSingleton<ITesteA, TesteA>();
        services.AddTransient<ITesteB, TesteB>();
        services.AddScoped<TesteC>();
        services.AddTransient<TesteInjecao>();
    })
    .Build();

host.Run();