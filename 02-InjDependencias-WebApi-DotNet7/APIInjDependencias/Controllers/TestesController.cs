using Microsoft.AspNetCore.Mvc;
using APIInjDependencias.Models;

namespace APIInjDependencias.Controllers;

[ApiController]
[Route("[controller]")]
public class TestesController : ControllerBase
{
    private readonly ILogger<TestesController> _logger;

    public TestesController(ILogger<TestesController> logger)
    {
        _logger = logger;
    }

    [HttpGet(nameof(GetFromServices))]
    public ResultadoTeste GetFromServices([FromServices]InfoInicializacao inicializacao)
    {
        var horarioAtual = $"{DateTime.Now:HH:mm:ss}";
        var action = nameof(GetFromServices);   
        _logger.LogInformation($"{action} acionado em {horarioAtual}");
        
        return new ()
        {
            Action = action,
            HorarioAtual = horarioAtual,
            HorarioInicializacao = inicializacao.Horario,
            Framework = inicializacao.Framework
        };
    }

    [HttpGet(nameof(GetWithoutFromServices))]
    public ResultadoTeste GetWithoutFromServices(InfoInicializacao inicializacao)
    {
        var horarioAtual = $"{DateTime.Now:HH:mm:ss}";
        var action = nameof(GetWithoutFromServices);   
        _logger.LogInformation($"{action} acionado em {horarioAtual}");
        
        return new ()
        {
            Action = action,
            HorarioAtual = horarioAtual,
            HorarioInicializacao = inicializacao.Horario,
            Framework = inicializacao.Framework
        };
    }
}