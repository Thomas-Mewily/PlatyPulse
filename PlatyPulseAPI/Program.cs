global using ChallengeID   = System.Guid;
global using ObjectifID    = System.Guid;
global using UserID        = System.Guid;
global using ChallengeEntryID = System.Guid;
global using ObjectifEntryID  = System.Guid;

using System.Diagnostics.Metrics;

namespace PlatyPulseAPI;

public class AppContext
{
    public Application App => Application.Instance;
    public DateTime    CurrentTime => App.CurrentTime;
}

public class Application : AppContext
{
    public static Application Instance { get; private set; } = new Application();
    public new DateTime CurrentTime;
}



public enum ChallengePeriod 
{
    Past,
    Active,
    Futur,
}

public class Challenge : AppContext
{
    /// ================= Fields =========
    public ChallengeID ID { get; set; } = ChallengeID.NewGuid();

    public DateTime Begin { get; set; } = DateTime.Now;
    public TimeSpan Duration { get; set; } = TimeSpan.FromDays(1);

    List<Objectif> Objectifs { get; set; } = new();

    /// ================= Properties =========
    DateTime End => Begin + Duration;

    TimeSpan TimeRemaning => End - CurrentTime;
    ChallengePeriod Period 
    { 
        get 
        { 
            if (IsActive) { return ChallengePeriod.Active; }
            if (IsPast  ) { return ChallengePeriod.Past  ; }
            if (IsFutur ) { return ChallengePeriod.Futur ; }
            throw new Exception("pas possible");
        }
    }

    public bool IsActive => CurrentTime >= Begin && CurrentTime < End;
    public bool IsPast => End < CurrentTime;
    public bool IsFutur => Begin >= CurrentTime;

    private Challenge() { }
    public static Challenge Daily(List<Objectif> objectifs) { var c = new Challenge(); c.Objectifs = objectifs; return c; }
}

/// <summary>
/// A participation for a person in a challenge
/// </summary>
public class ChallengeEntry : AppContext
{
    public ChallengeEntryID ID;
    public User     User;
    public Score    PlayerScore;
    public int      GoalIdx;
    public Objectif Objectifs;
}

/// <summary>
/// A participation for a person in an objectif
/// </summary>
public class ObjectifEntry : AppContext
{
    public ObjectifEntryID ID;
    public User User;
    public Objectif Objectif;
    //Challenge Objectifs;
}

public struct Score 
{
    double Value;
    private Score(double value) { Value = value; }

    public static Score Meter(double value) => new Score(value);
    public static Score Pompe(double value) => new Score(value);
}

public enum ObjectifKind 
{
    Courrir,
    Pompe,
}

/// <summary>
/// Description of a challenge
/// </summary>
public class Objectif : AppContext
{
    public ObjectifID    ID;
    public ObjectifKind  Kind;
    public TimeSpan?     MaxTime;
    public List<Goal>    Goals;
}

public class Goal : AppContext
{
    Score    ScoreToReach;
    XP       Reward;
}


public class User : AppContext
{
    UserID IP;
    Pseudo Pseudo;

    DateTime CreationData;
    DateTime Birthday;

    XP XP;

    public User(UserID iP, Pseudo pseudo, DateTime creationData, DateTime birthday, XP xP)
    {
        IP = iP;
        XP = xP;
        Pseudo = pseudo;
        CreationData = creationData;
        Birthday = birthday;
    }
}

public record Pseudo
{
    string Name;
    /// <summary>
    /// Stored in ascii order
    /// </summary>
    const string AllowedChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZ_abcdefghijklmnopqrstuvxwyz";

    private Pseudo(string name)
    {
        Name = name;
    }

    public Pseudo? New(string name) 
    { 
        return name.Length <= 16 && name.All(x => Pseudo.AllowedChar.Contains(x)) ? new Pseudo(name) : null;
    }
}

public record XP
{
    int value;

    public XP(int value)
    {
        this.value = value;
    }
}