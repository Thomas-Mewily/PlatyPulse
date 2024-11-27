using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BetterCSharp;

public static class Json
{
    public static readonly JsonSerializerOptions Option;
    static Json()
    {
        Option = new JsonSerializerOptions();
        Option.PropertyNameCaseInsensitive = true;
    }

    public static string ToJson<T>(this T o) => JsonSerializer.Serialize(o, Option);
    public static T? TryFrom<T>(string json) => JsonSerializer.Deserialize<T>(json, Option);

    public static T From<T>(string json) => TryFrom<T>(json).Unwrap();
}

