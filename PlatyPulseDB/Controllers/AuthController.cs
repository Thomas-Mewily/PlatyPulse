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
public class AuthController(DataBaseCtx db, IConfiguration config) : PlatyController(db, config)
{
    [HttpPost("register")]
    public ActionResult<UserLogged> Register(UserRegister request) 
    {
        try 
        {
            var password = request.Password.CheckPasswordRobust();
            var pseudo = request.Pseudo.ToPseudo().Check();
            var email = request.Email.ToEmail().Check();
            var passwordHashed = BCrypt.Net.BCrypt.HashPassword(password + SEL);

            if (!Db.EmailAvailable(request.Email.ToEmail()))
            {
                return BadRequest($"Email {request.Email} is already taken");
            }

            var user = new User(pseudo, email, passwordHashed, Role.Consumer, DateTime.Now, 0.XP());

            // Todo : what if the GUID already exist ? Don't erase an existing account
            // same for every add, maybe overload the add func
            Db.Add(user);
            Db.SaveChanges();
            return Login(new UserLogin(email.Address, request.Password));
            //return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public ActionResult<UserLogged> Login([FromQuery] UserLogin request)
    {
        try 
        {
            var u = GetAccount(request.Email);

            if (!BCrypt.Net.BCrypt.Verify(request.Password + SEL, u.PasswordHashed))
            {
                throw new Exception();
            }

            var logged_user = new UserLogged(u, CreateToken(u));
            return Ok(logged_user);
        }
        catch (Exception)
        {
            // Security : Don't tell what wrong, to avoid hacker to scrap user email

            return BadRequest("Wrong email or password");
        }
    }

    
    [HttpGet("validate-token")]
    public IActionResult ValidateToken([FromQuery] JWTString token)
    {
        try 
        {
            return Ok(CheckTokenAndGetEmail(token));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    
    [HttpGet("get_xp")]
    public ActionResult<string> GetXP([FromQuery] JWTString token)
    {
        try 
        {
            var u = CheckTokenAndGetUser(token);
            return Ok(u.XP.ToJson());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("user_data")]
    public ActionResult<User> GetUserData([FromQuery] JWTString token)
    {
        try
        {
            return Ok(CheckTokenAndGetUser(token).ToJson());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}