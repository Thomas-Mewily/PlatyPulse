using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PlatyPulseAPI;

public static class Json
{
    private static readonly JsonSerializerOptions JsonOption;
    static Json()
    {
        JsonOption = new JsonSerializerOptions();
    }

    public static string ToJson<T>(this T o) => JsonSerializer.Serialize(o, JsonOption);

    public static T? TryFromJson<T>(string json) 
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, JsonOption);
        }
        catch (Exception) 
        { 
            return default;  
        }
    }
    public static T FromJson<T>(string json) => TryFromJson<T>(json).Unwrap();
}

