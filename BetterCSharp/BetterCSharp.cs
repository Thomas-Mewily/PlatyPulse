namespace BetterCSharp;

public static class Z 
{
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
    public static T Unwrap<T>(this T? t, string? error = null)
        => (t == null) ? throw new ArgumentNullException(error != null ? error : typeof(T) + " was null") : t!;


    /// <summary>
    /// Throw a new Exception
    /// </summary>
    /// <param name="o"></param>
    /// <exception cref="Exception"></exception>
    public static void Panic<T>(this T o) => throw new Exception(o != null ? o.ToString() : "Panic !");

    public static void Todo() => "Todo".Panic();

    public static T Default<T>() where T : new() => new();

    /*
    public static T? FirstOrNone<T>(this IEnumerable<T> i, Func<T, bool> predicate) where T : class
    {
        var f = i.FirstOrDefault(predicate);
        return f;
    }*/
}