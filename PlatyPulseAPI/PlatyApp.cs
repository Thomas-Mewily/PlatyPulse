using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;
using System.ComponentModel.DataAnnotations.Schema;
using BetterCSharp;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace PlatyPulseAPI;

public partial class PlatyApp : PlatyAppComponent
{
    public static PlatyApp Instance { get; private set; } = new PlatyApp();

    [NotMapped]
    [JsonIgnore]
    public new DateTime CurrentTime { get; private set; } = DateTime.Now;

    [NotMapped]
    [JsonIgnore]
    public new UserLogged LoggedUser { get; private set; } = new();

    [NotMapped]
    [JsonIgnore]
    public new JWTString JWT => LoggedUser.JWT;

    [NotMapped]
    [JsonIgnore]
    public new User? MaybeCurrentUser => LoggedUser.User;

    [NotMapped]
    [JsonIgnore]
    public new HttpClient WebClient { get; private set; } = new();

    public new User CurrentUser() => MaybeCurrentUser.Unwrap();

    [NotMapped]
    [JsonIgnore]
    public new Challenge DailyChallenge { get; private set; } = new();

    [NotMapped]
    [JsonIgnore]
    public ReadOnlyDictionary<ChallengeID, Challenge> AllChallenges => _AllChallenges.AsReadOnly();
    [NotMapped]
    [JsonIgnore]
    public Dictionary<ChallengeID, Challenge> _AllChallenges = [];

    [NotMapped]
    [JsonIgnore]
    public ReadOnlyDictionary<ChallengeEntryID, ChallengeEntry> AllChallengesEntries => _AllChallengesEntries.AsReadOnly();
    [NotMapped]
    [JsonIgnore]
    public Dictionary<ChallengeEntryID, ChallengeEntry> _AllChallengesEntries = [];

    [NotMapped]
    [JsonIgnore]
    public ReadOnlyDictionary<QuestID, Quest> AllQuests { get => _AllQuests.AsReadOnly(); }
    [NotMapped]
    [JsonIgnore]
    public Dictionary<QuestID, Quest> _AllQuests = [];

    [NotMapped]
    [JsonIgnore]
    public ReadOnlyDictionary<QuestEntryID, QuestEntry> AllQuestsEntries => _AllQuestsEntries.AsReadOnly();
    [NotMapped]
    [JsonIgnore]
    public Dictionary<QuestEntryID, QuestEntry> _AllQuestsEntries = [];

    [NotMapped]
    [JsonIgnore]
    public ReadOnlyDictionary<UserID, User> AllUser => _AllUser.AsReadOnly();
    [NotMapped]
    [JsonIgnore]
    public Dictionary<UserID, User> _AllUser = [];

    [NotMapped]
    [JsonIgnore]
    public new bool IsConnected => MaybeCurrentUser != null;
}


public partial class PlatyApp : PlatyAppComponent
{
    private static string ServerApiURL = "https://localhost:7021/";

    public new void LogOut() => LoggedUser.Disconnect();

    private string GetUrl(string entry_point) => ServerApiURL + "api/" + entry_point;

    private async Task<HttpResponseMessage?> _DbPostAsync(string entry_point, string content = "")
    {
        var url = GetUrl(entry_point);
        var web_content = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await WebClient.PostAsync(url, web_content);
        if (!response.IsSuccessStatusCode) { throw new Exception("Post error " + response.StatusCode + " : " + response.Content.ToString());  }
        return response;
    }
    public async Task<R> DbPost<C, R>(string url, C content)
    {
        string content_json = content.ToJson();
        var web_result = _DbPostAsync(url, content_json);
        var reponse = (await web_result).Unwrap();
        string result_json = await reponse.Content.ReadAsStringAsync();
        Console.WriteLine("Post : " + result_json);

        var result = Json.From<R>(result_json);
        Console.WriteLine("Post Result : " + result);
        return result;
    }

    private async Task<HttpResponseMessage?> _DbGetAsync(string entry_point, JWTString token = "")
    {
        var url = GetUrl(entry_point) + "?token=" + Uri.EscapeDataString(token);
        var response = await WebClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) { throw new Exception("Get error " + response.StatusCode + " : " + response.Content.ToString()); }
        Console.WriteLine("Get : " + response.ToString());
        return response;
    }
    public async Task<R> DbGetAsync<R>(string url, JWTString? token = null)
    {
        var web_result = _DbGetAsync(url, token ?? JWT);
        var reponse = (await web_result).Unwrap();
        string result_json = await reponse.Content.ReadAsStringAsync();
        Console.WriteLine("Get : " + result_json);

        var result = Json.From<R>(result_json);
        return result.Unwrap();
    }

    public async Task<User> DbGetUserData(JWTString? token = null) => await DbGetAsync<User>("user_data", token);

    private void LoggedAs(UserLogged user) 
    { 
        LogOut();
        LoggedUser = user;
    }

    public new async Task<bool> LogIn(Email email, string mdp) => await LogIn(new UserLogin(email.Address, mdp));
    public new async Task<bool> LogIn(UserLogin login)
    {
        try
        {
            login.Email.ToEmail().Check();
            LoggedAs(await DbPost<UserLogin, UserLogged>("Auth/login", login));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public new async Task<bool> Register(string email, string pseudo, string mdp) => await Register(new UserRegister(email, pseudo, mdp));

    /// <summary>
    /// Regster and loggin the new created user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="pseudo"></param>
    /// <param name="mdp"></param>
    /// <returns></returns>
    public new async Task<bool> Register(UserRegister register)
    {
        register.Email.ToEmail().Check();
        register.Pseudo.ToPseudo().Check();
        register.Password.CheckPasswordRobust();

        var user_logged = await DbPost<UserRegister, UserLogged>("Auth/register", register);
        LoggedAs(user_logged);
        return true;
    }


    //public bool LogInAsAdmin() => 

    public void LoadExample() 
    {
        var run =
                new Quest(QuestKind.Run,
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
        new Quest(QuestKind.PushUp,
            [
                new Rank(1.PushUp(), 1.XP()),
                new Rank(3.PushUp(), 5.XP()),
                new Rank(10.PushUp(), 30.XP()),
                new Rank(20.PushUp(), 100.XP()),
            ]
        );

        var c = Challenge.Daily([run, push_up]);
        //c.ServerUpload();
        //AddChallenge(c);
        //App.DailyChallenge = c;
    }

    public void Update() 
    {
        var old_time = CurrentTime;
        CurrentTime = DateTime.Now;

        if (old_time.DayOfWeek != CurrentTime.DayOfWeek) 
        {
            
        }
    }

    public static void InitJsonSerializerOptions(JsonSerializerOptions? option = null) 
    {
        var o = option ?? Json.Option;
        o.Converters.Add(new XPJsonConverter());
        o.Converters.Add(new EmailJsonConverter());
        o.Converters.Add(new PseudoJsonConverter());
    }
}

/// <summary>
///  Contains only properties to access the application data
/// </summary>
public class PlatyAppComponent
{

#pragma warning disable CA1822 // Marquer les membres comme étant static
    [NotMapped] [JsonIgnore]
    public PlatyApp App => PlatyApp.Instance;
#pragma warning restore CA1822 // Marquer les membres comme étant static
    [NotMapped] [JsonIgnore]
    public DateTime CurrentTime => App.CurrentTime;

    [NotMapped] [JsonIgnore]
    public UserLogged LoggedUser => App.LoggedUser;
    [NotMapped] [JsonIgnore]
    public JWTString JWT => App.JWT;
    [NotMapped] [JsonIgnore]
    public User? MaybeCurrentUser => App.MaybeCurrentUser;
    public User  CurrentUser() => App.CurrentUser();

    [NotMapped] [JsonIgnore]
    public Challenge DailyChallenge => App.DailyChallenge;
    [NotMapped] [JsonIgnore]
    public HttpClient WebClient => App.WebClient;


    [NotMapped] [JsonIgnore]
    public bool IsConnected => App.IsConnected;

    public void LogOut() => App.LogOut();

    public new async Task<bool> LogIn(Email email, string mdp) => await App.LogIn(email, mdp);
    public new async Task<bool> LogIn(UserLogin login) => await App.LogIn(login);
    public new async Task<bool> LogIn(JWTString token) => await App.LogIn(token);

    public new async Task<bool> Register(string email, string pseudo, string mdp) => await App.Register(email, pseudo, mdp);
    public new async Task<bool> Register(UserRegister register) => await App.Register(register);



    // Challenge
    //public Challenge? ObserveChallenge(ChallengeID challengeID) => App.AllChallenges.GetOrNull(challengeID);
    /*
    public ChallengeID AddChallenge(Challenge challenge)
    {
        if (challenge.ID == ChallengeID.Empty)
        {
            challenge.ID = ChallengeID.NewGuid();
            App.AllChallenges.Add(challenge.ID, challenge);
        }

        foreach(var q in challenge.Quests) 
        {
            AddQuest(q);
        }
        return challenge.ID;
    }*/

    /*
    public ChallengeEntry? ObserveChallengeEntry(ChallengeEntryID challengeEntryID) => App.AllChallengesEntries.GetOrNull(challengeEntryID);
    public QuestEntryID AddChallengeEntry(ChallengeEntry e) 
    { 
        
    }

    // Quest
    public Quest? ObserveQuest(QuestID questID) => App.AllQuests.GetOrNull(questID);
    public QuestID AddQuest(Quest quest) 
    {
        if (quest.ID == QuestID.Empty) 
        {
            quest.ID = QuestID.NewGuid();
            App.AllQuests.Add(quest.ID, quest);
        }
        return quest.ID;
    }

    // QuestEntry
    public QuestEntry? ObserveQuestEntry(QuestEntryID questEntryID) => App.AllQuestsEntries.GetOrNull(questEntryID);
    */
    public PlatyAppComponent() { }
}