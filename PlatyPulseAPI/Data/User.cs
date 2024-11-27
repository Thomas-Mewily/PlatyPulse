using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public enum Role
{
    Consumer,
    Admin,
}

/// <summary>
/// Represent a person, entity that can be logged
/// </summary>
public class User : IdentifiableData
{
    /// ================= Fields =========
    public Pseudo Pseudo { get; set; } = new();
    [JsonIgnore]
    public Email  Email { get; set; } = new();
    [JsonIgnore]
    public string PasswordHashed { get; set; } = string.Empty;
    public UserID UserID { get; set; } = UserID.NewGuid();

    public Role Role { get; set; } = Role.Consumer;

    public DateTime CreationDate { get; set; } = DateTime.Now;
    public XP XP { get; set; } = XP.Zero;


    public override void ForceUpdateFrom(IdentifiableData other)
    {
        var u = (other as User).Unwrap();
        Pseudo = u.Pseudo;
        // Email and Password can't be edited here because of security

        // To test, todo : delete it
        XP = u.XP;
    }

    /// ================= Rest =========

    public User() { }
    public User(Role role) : this() { Role = role;  }
    public User(Pseudo pseudo, Email email, string passwordHashed, Role role, DateTime creationDate, XP xP)
    {
        Pseudo = pseudo;
        Email = email;
        PasswordHashed = passwordHashed;
        Role = role;
        CreationDate = creationDate;
        XP = xP;
    }

    [NotMapped]
    [JsonIgnore]
    public bool IsAdmin => Role == Role.Admin;
    [NotMapped]
    [JsonIgnore]
    public bool IsNotAdmin => !IsAdmin;

    protected override bool _CanBeEditedBy(User u) => u.ID == ID;

    public override string ToString() => (IsAdmin ? "admin " : "") + Pseudo + "#" + ID;

    public static User TestDefault => new();
    public static User TestDefaultAdmin => new(Role.Admin);




    // We don't need that for the moment
    // public DateTime Birthday { get; set; } = new DateTime(1000);



    //public override bool IsPrivateData => true;
}

public class UserRegister
{
    public string Email { get; set; } = "";
    public string Pseudo { get; set; } = "";
    public string Password { get; set; } = "";

    public UserRegister() { }
    public UserRegister(string email, string pseudo, string password) { Email = email; Pseudo = pseudo;  Password = password; }

    public override string ToString() => Pseudo + " / " + Email + " / " + Password;
}

public class UserLogin
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    public UserLogin() { }
    public UserLogin(string email, string password) { Email = email; Password = password; }

    public override string ToString() => Email + " / " + Password;
}

public class UserLogged
{
    public User User { get; set; } = new();
    public JWTString JWT { get; set; } = "";

    [NotMapped]
    [JsonIgnore]
    public bool IsConnected => JWT.Length > 0;

    public UserLogged() { }
    public UserLogged(User user, string jwt)
    {
        User = user;
        JWT = jwt;
    }

    public void Disconnect() 
    {
        User = new();
        JWT = "";
    }

    public override string ToString() => User + " : " + JWT;
}