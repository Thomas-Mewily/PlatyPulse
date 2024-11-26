global using ID = System.Guid;
global using ChallengeID = System.Guid;
global using QuestID = System.Guid;
global using UserID = System.Guid;
global using ChallengeEntryID = System.Guid;
global using QuestEntryID = System.Guid;
global using Meter = double;
global using PushUp = int;

global using BetterCSharp;

using System.Diagnostics;
using System.Text.Json.Serialization;
using PlatyPulseAPI.Data;
using System.Collections.ObjectModel;
using PlatyPulseAPI.Value;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatyPulseAPI;

public class PlatyApp : PlatyAppComponent
{
    public static PlatyApp Instance { get; private set; } = new PlatyApp();

    public new DateTime CurrentTime { get; private set; } = DateTime.Now;

    public new User? CurrentUser { get; private set; }
    public new Challenge DailyChallenge { get; private set; } = new();

    public ReadOnlyDictionary<ChallengeID, Challenge> AllChallenges => _AllChallenges.AsReadOnly();
    public Dictionary<ChallengeID, Challenge> _AllChallenges = [];

    public ReadOnlyDictionary<ChallengeEntryID, ChallengeEntry> AllChallengesEntries => _AllChallengesEntries.AsReadOnly();
    public Dictionary<ChallengeEntryID, ChallengeEntry> _AllChallengesEntries = [];

    public ReadOnlyDictionary<QuestID, Quest> AllQuests { get => _AllQuests.AsReadOnly(); }
    public Dictionary<QuestID, Quest> _AllQuests = [];

    public ReadOnlyDictionary<QuestEntryID, QuestEntry> AllQuestsEntries => _AllQuestsEntries.AsReadOnly();
    public Dictionary<QuestEntryID, QuestEntry> _AllQuestsEntries = [];

    public ReadOnlyDictionary<UserID, User> AllUser => _AllUser.AsReadOnly();
    public Dictionary<UserID, User> _AllUser = [];

    public new bool IsConnected => CurrentUser != null;
    public new void LogOut() { CurrentUser = null; }

    public new bool LogIn(string email, string mdp) { "get the user id can connect to it".Panic(); return true; }
    public new bool LogIn(UserID id, string mdp) 
    {
        // Todo : check mdp / password, and retrive user information
        return LoggedAs(new User(id));
    }

    private bool LoggedAs(User user)
    {
        CurrentUser = user;
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

        LoggedAs(User.TestDefaultAdmin);
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
    public User? CurrentUser => App.CurrentUser;
    [NotMapped] [JsonIgnore]
    public Challenge DailyChallenge => App.DailyChallenge;

    [NotMapped] [JsonIgnore]
    public bool IsConnected => App.IsConnected;
    public void LogOut() => App.LogOut();

    public bool LogIn(string email, string mdp) => App.LogIn(email, mdp);
    public bool LogIn(UserID id, string mdp) => App.LogIn(id, mdp);


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