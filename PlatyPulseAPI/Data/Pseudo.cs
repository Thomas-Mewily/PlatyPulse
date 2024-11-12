using System.Collections;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public static class PseudoExtension
{
    public static Pseudo ToPseudo(this string name) => new(name);
    public static bool PseudoIsValid(this string name) => Pseudo.IsValid(name);
}

public record Pseudo
{
    public string Name { get; set; }

    public static Pseudo Default => new("undefined");

    /// <summary>
    /// Stored in ascii order
    /// </summary>
    const string AllowedChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZ_abcdefghijklmnopqrstuvxwyz";

    public Pseudo(string name)
    {
        if (name.PseudoIsValid())
        {
            Name = name;
        }
        else
        {
            Name = "John_Doe";
        }
    }

    public static bool IsValid(string name) => name.Length <= 16 && name.All(c => AllowedChar.Contains(c));
    public override string ToString() => Name;
}
