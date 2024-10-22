global using ChallengeID   = System.Guid;
global using GoalID   = System.Guid;
global using UserID        = System.Guid;
global using ChallengeEntryID = System.Guid;
global using GoalEntryID = System.Guid;
global using Meter = double;
global using PushUp = int;
using System.Diagnostics;

namespace PlatyPulseAPI;

public class Application : ApplicationContext
{
    public static Application Instance { get; private set; } = new Application();
    public new DateTime CurrentTime = DateTime.Now;

    public new User? CurrentUser;
    public new Challenge DailyChallenge = Challenge.Default;

    public Dictionary<ChallengeID, Challenge> AllChallenges = [];
    public Dictionary<ChallengeEntryID, ChallengeEntry> AllChallengesEntries = [];

    public Dictionary<GoalID, Goal> AllGoals = [];
    public Dictionary<GoalEntryID, GoalEntry> AllGoalsEntries = [];

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
public class ApplicationContext
{
    public Application App => Application.Instance;

    public DateTime CurrentTime => App.CurrentTime;
    public User? CurrentUser => App.CurrentUser;
    public Challenge DailyChallenge => App.DailyChallenge;


    public Challenge? ObserveChallenge(ChallengeID challengeID) => App.AllChallenges.GetOrNull(challengeID);
    public ChallengeID AddChallenge(Challenge challenge)
    {
        if (challenge.ID == ChallengeID.Empty)
        {
            challenge.ID = ChallengeID.NewGuid();
            App.AllChallenges.Add(challenge.ID, challenge);
        }
        foreach(var goal in challenge) 
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

    public ApplicationContext() { }
}