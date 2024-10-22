using PlatyPulseAPI;
namespace PlatiPulseAPITest;

public class Program : ApplicationContext
{
    static void Main(string[] args)
    {
        var p = new Program();
    }

    public Program() 
    {
        var run = 
            new Goal(GoalKind.Run, 
                [
                new Rank(100.Meter(), 10.XP()), 
                new Rank(200.Meter(), 30.XP()), 
                new Rank(500.Meter(), 70.XP()),
                new Rank(1.KiloMeter(), 1500.XP()),
                new Rank(3.KiloMeter(), 3500.XP()),
                new Rank(5.KiloMeter(), 6500.XP()),
                ]
            );

        var push_up =
        new Goal(GoalKind.PushUp,
            [
            new Rank(1.PushUp(), 1.XP()),
                new Rank(3.PushUp(), 5.XP()),
                new Rank(10.PushUp(), 30.XP()),
                new Rank(20.PushUp(), 100.XP()),
            ]
        );

        var c = Challenge.Daily([run, push_up]);
        AddChallenge(c);
        App.DailyChallenge = c;
        Console.WriteLine(DailyChallenge);
    }
}
