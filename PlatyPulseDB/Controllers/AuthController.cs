using BetterCSharp;
using Microsoft.AspNetCore.Authorization;
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
            var passwordHashed = BCrypt.Net.BCrypt.HashPassword(password + Secret.Sel());

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
    public ActionResult<UserLogged> Login(UserLogin request)
    {
        try 
        {
            var u = GetUser(request.Email.ToEmail());

            if (!BCrypt.Net.BCrypt.Verify(request.Password + Secret.Sel(), u.PasswordHashed))
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

        
    [HttpGet("get_xp")]
    public ActionResult<string> GetXP()
    {
        try 
        {
            return Ok(CurrentUser.XP.ToJson());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [Authorize]
    [HttpGet("get_xp_logged")]
    public ActionResult<string> GetXPLogged()
    {
        try
        {
            return Ok(CurrentUser.XP.ToJson());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("user_data")]
    public ActionResult<User> GetUserData()
    {
        try
        {
            return Ok(CurrentUser.ToJson());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /*
    protected JWTString CreateToken(User acc)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, acc.Email.Address)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret.TopSecretToken()));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(14), signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }*/

    protected JWTString CreateToken(User u)
    {
        // Define the claims (use whatever claims you need here, this is just an example with email)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, u.Email.Address),
            new Claim(ClaimTypes.NameIdentifier, u.ID.ToString()),
        };

        // Secret key for signing the token (should be stored securely, not hardcoded in production)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret.TopSecretToken()));  // Make sure this key is long enough (at least 128 bits)

        // Create signing credentials
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Create the JWT token
        var token = new JwtSecurityToken(
            claims: claims,                      // Add claims
            expires: DateTime.Now.AddDays(14),   // Set expiration time (adjust this as needed)
            signingCredentials: creds           // Use the signing credentials
        );

        // Serialize the token to a string
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        // Return the token (can be a string or a custom JWTString type)
        return jwt;
    }
}