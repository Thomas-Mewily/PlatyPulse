using static System.Runtime.InteropServices.JavaScript.JSType;

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


    public static XP XP(this int i) => new XP(i);

    /// <summary>
    /// `T? => T or crash`
    /// 
    /// Transform an optional value into a non optional value.
    /// Crash if the value was null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T Unwrap<T>(this T? t, string error = "not supposed to be null")
        => (t == null) ? throw new ArgumentNullException(error) : t!;
}