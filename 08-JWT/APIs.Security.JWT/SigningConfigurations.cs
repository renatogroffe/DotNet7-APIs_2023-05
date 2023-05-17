using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APIs.Security.JWT;

public class SigningConfigurations
{
    public Guid Id { get; } = Guid.NewGuid();
    public SecurityKey Key { get; }
    public SigningCredentials SigningCredentials { get; }

    public SigningConfigurations(string secretJwtKey)
    {
        Key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretJwtKey));

        SigningCredentials = new(
            Key, SecurityAlgorithms.HmacSha256Signature);
    }
}