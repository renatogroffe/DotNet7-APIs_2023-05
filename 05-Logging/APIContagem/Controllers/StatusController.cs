using System.Net;
using Microsoft.AspNetCore.Mvc;
using APIContagem.Models;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private static bool _healthy = true;
    private readonly ILogger<StatusController> _logger;

    public StatusController(ILogger<StatusController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<StatusApi> Get()
    {
        var status = GetCurrentStatusApplication();
        if (_healthy)
        {
            _logger.LogInformation("Simulacao status = OK");
            return status;
        }
        else
        {
            _logger.LogError("Simulacao status = Unhealthy");
            return new ObjectResult(status)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpGet("healthy")]
    public ActionResult<StatusApi> SetHealthy()
    {
        _healthy = true;
        _logger.LogInformation("Novo status = Healthy");
        return GetCurrentStatusApplication();
    }

    [HttpGet("unhealthy")]
    public ActionResult<StatusApi> SetUnhealthy()
    {
        _healthy = false;
        _logger.LogWarning("Novo status = Unhealthy");
        return GetCurrentStatusApplication();
    }

    private StatusApi GetCurrentStatusApplication()
    {
        return new StatusApi
        {
            Producer = Environment.MachineName,
            Healthy = _healthy,
            Mensagem = _healthy ? "OK" : "Unhealthy"
        };
    }
}