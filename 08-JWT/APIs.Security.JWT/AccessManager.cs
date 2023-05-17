using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace APIs.Security.JWT;

public class AccessManager
{
    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;
    private SigningConfigurations _signingConfigurations;
    private TokenConfigurations _tokenConfigurations;

    public AccessManager(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        SigningConfigurations signingConfigurations,
        TokenConfigurations tokenConfigurations)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _signingConfigurations = signingConfigurations;
        _tokenConfigurations = tokenConfigurations;
    }

    public bool ValidateCredentials(User user)
    {
        bool credenciaisValidas = false;
        if (user is not null && !String.IsNullOrWhiteSpace(user.UserID))
        {
            // Verifica a existência do usuário nas tabelas do
            // ASP.NET Core Identity
            var userIdentity = _userManager
                .FindByNameAsync(user.UserID).Result;
            if (userIdentity is not null)
            {
                // Efetua o login com base no Id do usuário e sua senha
                var resultadoLogin = _signInManager
                    .CheckPasswordSignInAsync(userIdentity, user.Password!, false)
                    .Result;
                if (resultadoLogin.Succeeded)
                {
                    // Verifica se o usuário em questão possui
                    // a role Acesso-APIs
                    credenciaisValidas = _userManager.IsInRoleAsync(
                        userIdentity, Roles.ROLE_ACESSO_APIS!).Result;
                }
            }
        }

        return credenciaisValidas;
    }

    public Token GenerateToken(User user)
    {
        ClaimsIdentity identity = new(
            new GenericIdentity(user.UserID!, "Login"),
            new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserID!)
            }
        );

        DateTime dataCriacao = DateTime.Now;
        DateTime dataExpiracao = dataCriacao +
            TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _tokenConfigurations.Issuer,
            Audience = _tokenConfigurations.Audience,
            SigningCredentials = _signingConfigurations.SigningCredentials,
            Subject = identity,
            NotBefore = dataCriacao,
            Expires = dataExpiracao
        });
        var token = handler.WriteToken(securityToken);

        return new()
        {
            Authenticated = true,
            Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
            Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            AccessToken = token,
            Message = "OK"
        };
    }
}