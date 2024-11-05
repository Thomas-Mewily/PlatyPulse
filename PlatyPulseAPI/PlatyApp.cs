global using ID   = System.Guid;
global using ChallengeID      = System.Guid;
global using GoalID           = System.Guid;
global using UserID           = System.Guid;
global using ChallengeEntryID = System.Guid;
global using GoalEntryID      = System.Guid;
global using Meter = double;
global using PushUp = int;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI;

public class PlatyApp : PlatyAppComponent
{
    public static PlatyApp Instance { get; private set; } = new PlatyApp();
    public new DateTime CurrentTime = DateTime.Now;

    public new User? CurrentUser;
    public new Challenge DailyChallenge = Challenge.Default;

    public Dictionary<ChallengeID, Challenge> AllChallenges = [];
    public Dictionary<ChallengeEntryID, ChallengeEntry> AllChallengesEntries = [];

    public Dictionary<GoalID, Goal> AllGoals = [];
    public Dictionary<GoalEntryID, GoalEntry> AllGoalsEntries = [];

    public void LoadExample() 
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
    public PlatyApp App => PlatyApp.Instance;
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

        foreach(var goal in challenge.Goals) 
        {
            AddGoal(goal);
        }
        return challenge.ID;
    }

    public ChallengeEntry? ObserveChallengeEntry(ChallengeEntryID challengeEntryID) => App.AllChallengesEntries.GetOrNull(challengeEntryID);

    public Goal? ObserveGoal(GoalID goalID) => App.AllGoals.GetOrNull(goalID);
    public GoalID AddGoal(Goal goal) 
    {
        if (goal.ID == GoalID.Empty) 
        {
            goal.ID = GoalID.NewGuid();
            App.AllGoals.Add(goal.ID, goal);
        }
        return goal.ID;
    }

    public GoalEntry? ObserveGoalEntry(GoalEntryID goalEntryID) => App.AllGoalsEntries.GetOrNull(goalEntryID);

    public PlatyAppComponent() { }
}