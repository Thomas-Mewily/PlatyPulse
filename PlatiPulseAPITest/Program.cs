namespace PlatyPulseAPITest;

using BetterCSharp;
using PlatyPulseAPI;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;

public class Program : PlatyAppComponent
{
    static void Main()
    {
        PlatyApp.InitJsonSerializerOptions();
        _ = new Program();
    }
    
    private void TestDailyChallenge() 
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

    private void Wait() { Thread.Sleep(200);  }
    private void LongWait() { Thread.Sleep(7000); }


    private async void TestRegisterNewAccount()
    {
        var uid = new Random().Next(0, 10000);
        var email = "test_" + uid + "test@test";
        var pseudo = "Test_" + uid;
        var mdp   = "Test1234?" + uid;

        var register = new UserRegister(email, pseudo, mdp);

        Console.WriteLine("Registering " + register + "... ");
        var ok = await Register(email, pseudo, mdp);
        Console.WriteLine(ok + " : " + MaybeCurrentUser);
    }

    private void TestLoginAndEditXP(XP xp)
    {
        LongWait();
        LogIn("hello@test.test".ToEmail(), "Sami1234?");

        var user = MaybeCurrentUser.Unwrap();

        Console.WriteLine(MaybeCurrentUser + "have " + MaybeCurrentUser.XP);

        MaybeCurrentUser.XP = xp;
        MaybeCurrentUser.ServerUpload();

        Wait();
        MaybeCurrentUser.ServerDownload();

        Wait();
        Console.WriteLine(MaybeCurrentUser + "have now " + MaybeCurrentUser.XP);
        LogOut();
    }


    public Program()
    {
        var j = 10.XP().ToJson();

        LongWait();
        Console.WriteLine("Start " + DateTime.Now);
        //TestLoginAndEditXP(200.XP());
        TestRegisterNewAccount();

        LongWait();
        Console.WriteLine("I'm" + MaybeCurrentUser);

        Console.WriteLine("Done " + DateTime.Now);

    }
}
