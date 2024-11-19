using PlatyPulseAPI.Data;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Value;

public static class PseudoExtension
{
    public static OwnedPseudo ToOwnedPseudo(this string name) => name.ToPseudo().ToOwned();
    public static OwnedPseudo ToOwned(this Pseudo pseudo) => new(pseudo);

    public static Pseudo ToPseudo(this string name) => new(name);
}

public record Pseudo
{
    public const string DefaultPseudo = "undefined";
    public string Name { get; set; }

    /// <summary>
    /// Stored in ascii order
    /// </summary>
    const string AllowedChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZ_abcdefghijklmnopqrstuvxwyz";

    public Pseudo() : this(DefaultPseudo) { }
    public Pseudo(string name) { Name = name; }

    [JsonIgnore]
    public bool IsValid => Name.Length <= 16 && Name.All(AllowedChar.Contains);
    public override string ToString() => Name;
}

