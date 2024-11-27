namespace PlatyPulseAPITest;

using BetterCSharp;
using PlatyPulseAPI;
using PlatyPulseAPI.Data;

public class Program : PlatyAppComponent
{
    static void Main()
    {
        _ = new Program();
    }

    public Program()
    {
        App.LoadExample();
        var d = DailyChallenge;
        Console.WriteLine(d);

        var json = d.ToJson();
        Console.WriteLine(json);
        Console.WriteLine();

        var from_json = Json.FromJson<Challenge>(json);
        Console.WriteLine(from_json);
        Console.WriteLine();
    }
}
