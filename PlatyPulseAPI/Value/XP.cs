using System.Text.Json.Serialization;
using System.Text.Json;

namespace PlatyPulseAPI.Value;

public record XP(int Value)
{
    public static XP Zero { get; private set; } = new XP(0);
    public int Value { get; set; } = Value;

    public override string ToString()
    {
        if (Value <= 1000) { return Value + " xp"; }
        return Value / 1000.0 + "K xp";
    }

    public static XP operator +(XP left, XP right) => new(left.Value + right.Value);
    public static XP operator -(XP left, XP right) => new(left.Value - right.Value);
    public static XP operator -(XP left) => new(-left.Value);
    public static XP operator *(XP left, int right) => new(left.Value * right);
}

public class XPJsonConverter : JsonConverter<XP>
{
    public override XP Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int value))
        {
            return new XP(value);
        }
        throw new JsonException("Invalid JSON for XP");
    }

    public override void Write(Utf8JsonWriter writer, XP value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}