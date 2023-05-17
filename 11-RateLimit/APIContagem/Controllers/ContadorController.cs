using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using APIContagem.Models;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting("fixed")]
public class ContadorController : ControllerBase
{
    private static readonly Contador _CONTADOR = new Contador();
    private readonly ILogger<ContadorController> _logger;
    private readonly IConfiguration _configuration;

    public ContadorController(ILogger<ContadorController> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public ResultadoContador Get()
    {
        int valorAtualContador;

        lock (_CONTADOR)
        {
            _CONTADOR.Incrementar();
            valorAtualContador = _CONTADOR.ValorAtual;
        }

        _logger.LogInformation($"Contador - Valor atual: {valorAtualContador} | " +
            $"{DateTime.Now:HH:mm:ss}");

        return new()
        {
            ValorAtual = valorAtualContador,
            Producer = _CONTADOR.Local,
            Kernel = _CONTADOR.Kernel,
            Framework = _CONTADOR.Framework,
            Mensagem = _configuration["MensagemVariavel"]
        };
    }
}