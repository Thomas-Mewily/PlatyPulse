global using ID = System.Guid;
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

// Thank to this tutorial https://www.youtube.com/watch?v=UwruwHl3BlU

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataBaseCtx Db;
    private readonly IConfiguration Config;

    public AuthController(DataBaseCtx db, IConfiguration config) 
    {
        Db = db;
        Config = config;
    }

    [HttpPost("register")]
    public ActionResult<Account> Register(AccountDTO request) 
    {
        // Todo hash the password here
        // Todo : make sure the username is valid
        // Todo : make sure the password is also valid and secure

        var acc = new Account() 
        { 
            Username = request.Username,
            PasswordHashed = BCrypt.Net.BCrypt.HashPassword(request.Password + "LeSelJaiPerduAuJeu")
        };

        if (!Db.UserNameAvailable(request.Username)) 
        {
            return BadRequest($"Username {request.Username} is already taken");
        }

        while (Db.Account.Find(acc.ID) != null) 
        {
            acc.GenerateNewID();
        }

        Db.Add(acc);
        Db.SaveChanges();
        return Ok(acc);
    }

    [HttpGet("login")]
    public ActionResult<string> Login([FromQuery] AccountDTO request)
    {
        var account = Db.Account.FirstOrDefault(a => a.Username == request.Username);
        if (account == null)
        {
            return BadRequest("Account not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password + "LeSelJaiPerduAuJeu", account.PasswordHashed))
        {
            return BadRequest("Wrong password");
        }

        string token = CreateToken(account);
        return Ok(token);
    }


    private string CreateToken(Account acc) 
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, acc.Username)
        };

        var token_path = "AppSettings:Token";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetSection(token_path).Value.Unwrap(token_path)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}
