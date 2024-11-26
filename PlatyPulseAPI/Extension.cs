using PlatyPulseAPI.Value;
using System.Text.RegularExpressions;
namespace PlatyPulseAPI;

public static class Extension
{
    public static V? GetOrNull<K, V>(this Dictionary<K, V> self, K key) where K : notnull
            => self.TryGetValue(key, out V? value) ? value : default;


    public static Score PushUp(this int i) => Score.NewPushUp(i);

    public static Score Meter(this int i) => Score.NewMeter(i);
    public static Score Meter(this double i) => Score.NewMeter(i);

    public static Score KiloMeter(this int i) => Score.NewKiloMeter(i);
    public static Score KiloMeter(this double i) => Score.NewKiloMeter(i);


    public static XP XP(this int i) => new(i);


    private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", RegexOptions.Compiled);

    public static bool IsPasswordRobust(this string password)
    {
        return PasswordRegex.IsMatch(password);
    }
    public static string CheckPasswordRobust(this string password) 
    { 
        if (!password.IsPasswordRobust()) { throw new Exception("Password must contain at least eight characters, at least one number and both lower and uppercase letters and special characters"); }
        return password;
    }
}