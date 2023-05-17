using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionAppIsolatedDI.Interfaces;
using FunctionAppIsolatedDI.Implementations;

namespace FunctionAppIsolatedDI;

public class TestarInjDependencias
{
    private readonly ILogger _logger;
    private readonly TesteInjecao _objTesteInjecao;
    private readonly ITesteA _testeA;
    private readonly ITesteB _testeB;
    private readonly TesteC _testeC;

    public TestarInjDependencias(ILoggerFactory loggerFactory,
        TesteInjecao objTesteInjecao,
        ITesteA testeA,
        ITesteB testeB,
        TesteC testeC)
    {
        _logger = loggerFactory.CreateLogger<TestarInjDependencias>();
        _objTesteInjecao = objTesteInjecao;
        _testeA = testeA;
        _testeB = testeB;
        _testeC = testeC;
    }

    [Function("TestarInjDependencias")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation(
            $"C# HTTP trigger: executando o método Run da Function {nameof(TestarInjDependencias)}...");
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(_objTesteInjecao
            .RetornarValoresInjecao(_testeA, _testeB, _testeC));
        return response;
    }
}