namespace PlatyPulseAPITest;

using BetterCSharp;
using PlatyPulseAPI;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;

public class Program : PlatyAppComponent
{
    static async Task Main()
    {
        PlatyApp.InitJsonSerializerOptions();
        var p = new Program();
        await p.Test();
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

    private void Wait() { Thread.Sleep(3000);  }
    private void LongWait() { Thread.Sleep(7000); }


    private async Task TestRegisterNewAccount()
    {
        var uid = new Random().Next(0, 1000000);
        var email = "test_" + uid + "test@test";
        var pseudo = "Test_" + uid;
        var mdp   = "Test1234?" + uid;

        var register = new UserRegister(email, pseudo, mdp);

        Console.WriteLine("Registering " + register + "... ");
        await Register(email, pseudo, mdp);
    }

    private async Task SetXP()
    {
        Console.WriteLine(CurrentUser + " initaly have " + CurrentUser.XP);
        CurrentUser.XP += 10.XP();
        Console.WriteLine(CurrentUser + " have locally " + CurrentUser.XP);
        await CurrentUser.ServerUpdate();

        CurrentUser.XP = 0.XP();
        Console.WriteLine(CurrentUser + " have their xp locally reset" + CurrentUser.XP);

        await CurrentUser.ServerDownload();
        Console.WriteLine(CurrentUser + " have now " + CurrentUser.XP);
    }

    private async Task TestWaitForSwagger() 
    {
        LongWait();
        Console.WriteLine("Start at " + DateTime.Now);
        Console.WriteLine();

        //TestLoginAndEditXP(200.XP());
        await TestRegisterNewAccount();
        Console.WriteLine("I'm " + CurrentUser);
        Console.WriteLine();

        await SetXP();
        Console.WriteLine();


        Console.WriteLine();
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

    public Program() { }

    public async Task Test() 
    {
        //TestJson();
        //await TestWaitForSwagger();
        //await RecupererUtilisateur();
        await CreeQuest();
    }

    private async Task RecupererUtilisateur()
    {
        LongWait();

        var u = new User();
        u.ID = Guid.Parse("f2b1cfed-b47c-49f3-b4ac-860a461dc97a");
        Console.WriteLine(u);

        await u.ServerDownload();
        Console.WriteLine(u);
    }

    private async Task CreeQuest()
    {
        LongWait();

        await LogIn("admin@admin.fr".ToEmail(), "Admin1234!");

        var r = new List<Rank>()
        {
            new Rank(10.Meter(), 5.XP()),
            new Rank(50.KiloMeter(), 50.XP()),
        };
        var q = new Quest(QuestKind.Run, r, TimeSpan.FromDays(1.0));
        await q.ServerCreate();

        //await q.ServerUpdate();
    }
}
