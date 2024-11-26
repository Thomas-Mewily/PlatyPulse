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
public class AuthController : PlatyController
{
    public AuthController(DataBaseCtx db, IConfiguration config) : base(db, config) { }

    [HttpPost("register")]
    public ActionResult<Account> Register(AccountDTO request) 
    {
        // Todo hash the password here
        // Todo : make sure the username is valid
        // Todo : make sure the password is also valid and secure

        var acc = new Account() 
        { 
            Username = request.Username,
            PasswordHashed = BCrypt.Net.BCrypt.HashPassword(request.Password + SEL)
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

        if (!BCrypt.Net.BCrypt.Verify(request.Password + SEL, account.PasswordHashed))
        {
            return BadRequest("Wrong password");
        }

        string token = CreateToken(account);
        return Ok(token);
    }

    
    [HttpGet("validate-token")]
    public IActionResult ValidateToken([FromQuery] string token)
    {
        try 
        {
            return Ok(ValidateTokenAndGetUsername(token));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    
    [HttpGet("get_age")]
    public ActionResult<string> GetAge([FromQuery] string token)
    {
        try 
        {
            ValidateTokenAndGetUsername(token);
            return Ok("trop vieux (todo)");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

/*
username: thomas
mdp: sami
 */