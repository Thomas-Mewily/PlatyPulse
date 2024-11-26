using BetterCSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

    protected JWTString CreateToken(User acc)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, acc.Email.Address)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TopSecretToken()));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    //Db.Account.FirstOrDefault(a => a.Email == request.Email);
    protected User GetAccount(Email email) => Db.Account.FirstOrDefault(a => a.Email == email).Unwrap("No Account associated with " + email);
    protected User GetAccount(string email) => GetAccount(email.ToEmail());
    // Todo : not opti
    protected User GetAccount(ID id) => Db.Account.FirstOrDefault(a => a.ID == id).Unwrap("Can't find user " + id);



    protected void CheckToken(JWTString token) => CheckTokenAndGetEmail(token);
    protected User CheckTokenAndGetUser(JWTString token) => GetAccount(CheckTokenAndGetEmail(token));

    protected Email CheckTokenAndGetEmail(JWTString token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("JTW Token is missing.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TopSecretToken());

        try 
        {
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
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return email.ToEmail();
        }
        catch (Exception)
        {
            throw new Exception("JWT Token is invalid.");
        }
    }
}