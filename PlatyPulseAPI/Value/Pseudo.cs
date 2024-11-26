using PlatyPulseAPI.Data;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Value;

public static class PseudoExtension
{
    /*
    public static OwnedPseudo ToOwnedPseudo(this string name, UserID by) => name.ToPseudo().ToOwned(by);
    public static OwnedPseudo ToOwned(this Pseudo pseudo, UserID by) => new(pseudo, by);
    */

    public static Pseudo ToPseudo(this string name) => new(name);
}

public record Pseudo
{
    public const string DefaultPseudo = "undefined";
    public string Name { get; set; }

    public static string Recommendation => "Pseudo need to a a length between " + MinLength + " and " + MaxLength + " and can only contains: " + AllowedChar;
    public const int MinLength = 3;
    public const int MaxLength = 16;
    public const string AllowedChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZ_abcdefghijklmnopqrstuvxwyz";

    public Pseudo Check()
    {
        if (!IsValid) { throw new Exception(Recommendation); }
        return this;
    }

    public Pseudo() : this(DefaultPseudo) { }
    public Pseudo(string name) { Name = name; }

    [NotMapped] [JsonIgnore]
    public bool IsValid => Name.Length is >= MinLength and <= MaxLength && Name.All(AllowedChar.Contains);
    public override string ToString() => Name;
}

