
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions options;

    public JwtProvider(IOptions<JwtOptions> _options)
    {
        options = _options.Value;
    }
    public string GenerateToken(UserEntity user)
    {
        Claim[] claims =
        [
            new("UserId",user.Id.ToString())
        ];

        var keyBytes = Encoding.UTF8.GetBytes(options.SecretKey);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );
        
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(options.ExpiresHours)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}