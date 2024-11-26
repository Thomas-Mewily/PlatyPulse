using BetterCSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlatyPulseWebAPI.Controllers;

public class PlatyController : ControllerBase
{
    protected const string SEL = "LeSelJaiPerduAuJeu";

    protected readonly DataBaseCtx Db;
    protected readonly IConfiguration Config;

    public PlatyController(DataBaseCtx db, IConfiguration config)
    {
        Db = db;
        Config = config;
    }

    private string TopSecretToken()
    {
        var token_path = "AppSettings:Token";
        return Config.GetSection(token_path).Value.Unwrap(token_path);
    }

    protected string CreateToken(Account acc)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, acc.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TopSecretToken()));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    protected string ValidateTokenAndGetUsername(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("Token is missing.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TopSecretToken());

        // Validate the token
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // Set to true if you have an issuer to validate
            ValidateAudience = false, // Set to true if you have an audience to validate
            ClockSkew = TimeSpan.Zero // Optional: remove clock skew
        }, out SecurityToken validatedToken);

        // Extract claims
        var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (username == null)
        {
            throw new UnauthorizedAccessException("User ID not found in token.");
        }
        return username;
    }
}