﻿using PlatyPulseAPI.Data;
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
/// The private stuff of a user
/// </summary>
public class User : IdentifiableData
{
    public Pseudo Pseudo { get; set; } = new();
    [JsonIgnore]
    public Email  Email { get; set; } = new();
    [JsonIgnore]
    public string PasswordHashed { get; set; } = string.Empty;
    public UserID UserID { get; set; } = UserID.NewGuid();

    public Role Role { get; set; } = Role.Consumer;

    public DateTime CreationDate { get; set; } = DateTime.Now;
    public XP XP { get; set; } = XP.Zero;

    public override string ToString() => Pseudo + "#" + ID;

    public static User TestDefault => new();
    public static User TestDefaultAdmin => new(Role.Admin);

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

    // We don't need that for the moment
    // public DateTime Birthday { get; set; } = new DateTime(1000);



    //public override bool IsPrivateData => true;
}

public class UserRegister
{
    public required string Email { get; set; }
    public required string Pseudo { get; set; }
    public required string Password { get; set; }
}

public class UserLogin
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
