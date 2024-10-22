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
}