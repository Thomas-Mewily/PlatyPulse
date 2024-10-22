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
        Console.WriteLine(DailyChallenge);
    }
}
