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
    protected readonly DataBaseCtx Db;
    protected readonly IConfiguration Config;

    public PlatyController(DataBaseCtx db, IConfiguration config) : base() 
    {
        Db = db;
        Config = config;
    }

    public UserID UserID 
    { 
        get 
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) { throw new Exception("User ID not found in token."); }
            return Guid.Parse(userIdClaim);
        } 
    }

    private User? _CurrentUser = null;
    public User CurrentUser 
    {
        get 
        {
            if (_CurrentUser == null) { _CurrentUser = GetUser(UserID); }
            return _CurrentUser;
        }
    }

    protected User GetUser(Email email) => Db.Account.FirstOrDefault(a => a.Email == email).Unwrap("No Account associated with " + email);
    protected User GetUser(ID id) => Db.User.Find(id).Unwrap();

    protected void CheckSameID<T>(T t, ID id) where T : IdentifiableData 
    {
        if(t.ID == id) { return; } 
        throw new Exception("Missmatch id for " + typeof(T).Name + ", got " + id + " in the url but expected id " + t.ID);
    }
}