﻿global using ID = System.Guid;
using BetterCSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlatyPulseAPI;
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
    public ActionResult<User> Register(UserRegister request) 
    {
        // Todo : make sure the username is valid
        // Todo : make sure the password is also valid and secure

        try 
        {
            var password = request.Password.CheckPasswordRobust();
            var pseudo = request.Pseudo.ToPseudo().Check();
            var email = request.Email.ToEmail().Check();
            var passwordHashed = BCrypt.Net.BCrypt.HashPassword(password + SEL);

            if (!Db.EmailAvailable(request.Email.ToEmail()))
            {
                return BadRequest($"Username {request.Email} is already taken");
            }

            var user = new User(pseudo, email, passwordHashed, Role.Consumer, DateTime.Now, 0.XP());

            // Todo : what if the GUID already exist ? Don't erase an existing account
            // same for every add, maybe overload the add func
            Db.Add(user);
            Db.SaveChanges();
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("login")]
    public ActionResult<string> Login([FromQuery] UserLogin request)
    {
        try 
        {
            var account = GetAccount(request.Email);

            if (!BCrypt.Net.BCrypt.Verify(request.Password + SEL, account.PasswordHashed))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(account);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    [HttpGet("validate-token")]
    public IActionResult ValidateToken([FromQuery] string token)
    {
        try 
        {
            return Ok(ValidateTokenAndGetEmail(token));
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
            ValidateTokenAndGetEmail(token);
            return Ok("trop vieux (todo)");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}