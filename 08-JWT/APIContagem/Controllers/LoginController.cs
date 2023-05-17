using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIs.Security.JWT;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(Token), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public ActionResult<Token> Post(
        [FromBody] User usuario,
        [FromServices] ILogger<LoginController> logger,
        [FromServices] AccessManager accessManager)
    {
        logger.LogInformation($"Recebida solicitação para o usuário: {usuario?.UserID}");

        if (usuario is not null && accessManager.ValidateCredentials(usuario))
        {
            logger.LogInformation($"Sucesso na autenticação do usuário: {usuario.UserID}");
            return accessManager.GenerateToken(usuario);
        }
        else
        {
            logger.LogError($"Falha na autenticação do usuário: {usuario?.UserID}");
            return new UnauthorizedResult();
        }
    }
}