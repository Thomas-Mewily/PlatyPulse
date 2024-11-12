global using ID = System.Guid;
global using ChallengeID = System.Guid;
global using QuestID = System.Guid;
global using UserID = System.Guid;
global using ChallengeEntryID = System.Guid;
global using QuestEntryID = System.Guid;
global using Meter = double;
global using PushUp = int;
using System.Diagnostics;
using System.Text.Json.Serialization;
using PlatyPulseAPI.Data;

namespace PlatyPulseAPI;

public class PlatyApp : PlatyAppComponent
{
    public static PlatyApp Instance { get; private set; } = new PlatyApp();
    public new DateTime CurrentTime = DateTime.Now;

    public new User? CurrentUser;
    public new Challenge DailyChallenge = Challenge.Default;

    public Dictionary<ChallengeID, Challenge> AllChallenges = [];
    public Dictionary<ChallengeEntryID, ChallengeEntry> AllChallengesEntries = [];

    public Dictionary<QuestID, Quest> AllQuests = [];
    public Dictionary<QuestEntryID, QuestEntry> AllQuestsEntries = [];

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
        AddChallenge(c);
        App.DailyChallenge = c;
    }

    public void Update() 
    {
        var old_time = CurrentTime;
        CurrentTime = DateTime.Now;

        if (old_time.DayOfWeek != CurrentTime.DayOfWeek) 
        {
        
        }
    }

    private Challenge GetTodayDailyChallenge() 
    {
        return Challenge.Default;
    }
}

/// <summary>
///  Contains only properties to access the application data
/// </summary>
public class PlatyAppComponent
{
    [JsonIgnore]
#pragma warning disable CA1822 // Marquer les membres comme étant static
    public PlatyApp App => PlatyApp.Instance;
#pragma warning restore CA1822 // Marquer les membres comme étant static
    [JsonIgnore]
    public DateTime CurrentTime => App.CurrentTime;
    [JsonIgnore]
    public User? CurrentUser => App.CurrentUser;
    [JsonIgnore]
    public Challenge DailyChallenge => App.DailyChallenge;

    public Challenge? ObserveChallenge(ChallengeID challengeID) => App.AllChallenges.GetOrNull(challengeID);
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
    }

    public ChallengeEntry? ObserveChallengeEntry(ChallengeEntryID challengeEntryID) => App.AllChallengesEntries.GetOrNull(challengeEntryID);

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

    public QuestEntry? ObserveQuestEntry(QuestEntryID questEntryID) => App.AllQuestsEntries.GetOrNull(questEntryID);

    public PlatyAppComponent() { }
}