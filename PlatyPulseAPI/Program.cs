using System.Diagnostics.Metrics;

namespace PlatyPulseAPI;

public record ChallengesID { int id; }

public class Challenges
{
    DateTime Begin { get; }
    TimeSpan Duration { get; }
    DateTime End => Begin + Duration;

    ChallengesID ID;
    List<Challenge> All;

    //private Challenges(DateTime Begin, TimeSpan Duration,)
}

/// <summary>
/// Contient le score actuel pour un challenge
/// </summary>
public class Challenge
{
    Score PlayerScore;
    int   GoalIdx;
    ChallengeObjectif Objectif;
}

public struct Score 
{
    double Value;
    private Score(double value) { Value = value; }

    public static Score Meter(double value) => new Score(value);
    public static Score Pompe(double value) => new Score(value);
}

public enum ChallengeKind 
{
    Courrir,
    Pompe,
}

/// <summary>
/// Contient l'objectif du challenge
/// </summary>
public class ChallengeObjectif
{
    //public string      Name;
    public ChallengeKind Kind;
    public TimeSpan?     MaxTime;
    public List<Goal>    Goals;
}

public class Goal 
{
    Score    ScoreToReach;
    XP       Reward;
}


public class User
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

public record UserID
{
    int id;

    public UserID(int id)
    {
        this.id = id;
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