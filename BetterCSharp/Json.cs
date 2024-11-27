using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BetterCSharp;

public static class Json
{
    public static readonly JsonSerializerOptions Option;
    static Json()
    {
        Option = new JsonSerializerOptions();
    }

    public static string ToJson<T>(this T o) => JsonSerializer.Serialize(o, Option);

    public static T? TryFromJson<T>(string json) 
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, Option);
        }
        catch (Exception) 
        { 
            return default;  
        }
    }
    public static T FromJson<T>(string json) => TryFromJson<T>(json).Unwrap();
}

