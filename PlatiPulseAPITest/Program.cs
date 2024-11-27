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

        var from_json = Json.From<Challenge>(json);
        Console.WriteLine(from_json);
        Console.WriteLine();
    }

    private void Wait() { Thread.Sleep(200);  }
    private void LongWait() { Thread.Sleep(7000); }


    private async void TestRegisterNewAccount()
    {
        var uid = new Random().Next(0, 1000000);
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

    private void TestWaitForSwagger() 
    {
        LongWait();
        Console.WriteLine("Start " + DateTime.Now);
        //TestLoginAndEditXP(200.XP());
        TestRegisterNewAccount();

        LongWait();
        Console.WriteLine("I'm " + MaybeCurrentUser);

        Console.WriteLine("Done " + DateTime.Now);
    }

    private void TestJson() 
    {
        var logged = new UserLogged(new User("toto".ToPseudo(), "test0U@test.fr".ToEmail(), "?", Role.Consumer, DateTime.Now, 0.XP()), "lol");

        var json = logged.ToJson();
        Console.WriteLine("object : " + logged);
        Console.WriteLine("to json : " + json);

        var from_json_obj = Json.From<UserLogged>(json);
        Console.WriteLine("object from json : " + from_json_obj);
        Console.WriteLine("to json : " + from_json_obj.ToJson());

        Console.WriteLine();
        var hardcoded = Json.From<UserLogged>("{\"user\":{\"pseudo\":\"Test_896760\",\"role\":0,\"creationDate\":\"2024-11-28T00:04:14.9129901+01:00\",\"xp\":0,\"id\":\"87314440-5ad9-4819-9287-e8151b89ec77\"},\"jwt\":\"eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0Xzg5Njc2MHRlc3RAdGVzdCIsImV4cCI6MTczMzk1ODI1NX0.HosmftprjQ0FUb1ltKIwe87nREy8wtzDc3SC-DQ-ySSJRdvPlc7BuuMMwV-W86k3FRHa-64q-Gec7ZNn7btmWw\"}");
        Console.WriteLine(hardcoded);
    }

    public Program()
    {
        //TestJson();
        TestWaitForSwagger();
    }
}
