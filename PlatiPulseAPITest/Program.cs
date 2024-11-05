using PlatyPulseAPI;
namespace PlatiPulseAPITest;

public class Program : PlatyAppComponent
{
    static void Main(string[] args)
    {
        var p = new Program();
    }

    public Program() 
    {
        App.LoadExample();
        var d = DailyChallenge;
        Console.WriteLine(d);
        
        var t = new Test("nom2", "prenom2");
        Console.WriteLine(t.ToJson());
        Console.WriteLine();


        var json = d.ToJson();
        Console.WriteLine(json);
        Console.WriteLine();


        var from_json = Json.FromJson<Challenge>(json);
        Console.WriteLine(from_json);
        Console.WriteLine();

    }
}
