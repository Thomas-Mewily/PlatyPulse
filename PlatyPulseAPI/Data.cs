using System.Collections;
using System.Diagnostics.Metrics;
using static System.Net.WebRequestMethods;

namespace PlatyPulseAPI;

public enum ChallengePeriod 
{
    Past,
    Active,
    Futur,
}

public class Challenge : PlatyAppComponent, IEnumerable<Goal>
{
    public static Challenge Default { get; private set; } = new();

    /// ================= Fields =========
    public ChallengeID ID { get; set; } = ChallengeID.Empty;

    public DateTime Begin { get; set; } = DateTime.Now;
    public TimeSpan Duration { get; set; } = TimeSpan.FromDays(1);

    public List<Goal> Goals { get; set; } = [];

    /// ================= Properties =========
    public DateTime End => Begin + Duration;
    public TimeSpan TimeRemaning => End - CurrentTime;

    ChallengePeriod TimePeriod 
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
    public static Challenge Daily(List<Goal> objectifs) { var c = new Challenge(); c.Goals = objectifs; return c; }

    public IEnumerator<Goal> GetEnumerator() => ((IEnumerable<Goal>)Goals).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Goals).GetEnumerator();

    public override string ToString()
    {
        var time_str = "";
        switch (this.TimePeriod) 
        {
            case ChallengePeriod.Active: { time_str = "current";  } break;
            case ChallengePeriod.Futur: { time_str = "futur"; } break;
            case ChallengePeriod.Past: { time_str = "past"; } break;
        }

        var duration_str = this.Duration.TotalHours + " h";
        if (this.Duration == TimeSpan.FromDays(1)) { duration_str = "daily"; }
        if (this.Duration == TimeSpan.FromDays(7)) { duration_str = "weekly"; }

        return time_str + " " + duration_str + " challenge consist of " + string.Join(" and ", Goals.Select(x => x.ToString()));
    }
}

/// <summary>
/// A participation for a person in a challenge
/// </summary>
public class ChallengeEntry : PlatyAppComponent
{
    public ChallengeEntryID ID = ChallengeID.Empty;
    public User             User  = User.Default;
    public List<GoalEntry>  Goals = [];

    public ChallengeEntry() { }
    public ChallengeEntry(User user, List<GoalEntry> goals) : this(ChallengeID.NewGuid(), user, goals) { }
    public ChallengeEntry(ChallengeID id, User user, List<GoalEntry> goals)
    {
        ID = id;
        User = user;
        Goals = goals;
    }
}

/// <summary>
/// A participation for a person in an objectif
/// </summary>
public class GoalEntry : PlatyAppComponent
{
    public GoalEntryID ID;
    public User        User;
    public Score       Score;
    public Goal        Goal;
    public int         RankIdx;

    public GoalEntry(User user, Score score, Goal goal, int rankIdx = 0) : this(GoalEntryID.Empty, user, score, goal, rankIdx) { }
    public GoalEntry(ChallengeID id, User user, Score score, Goal goal, int rankIdx = 0)
    {
        ID = id;
        User = user;
        Score = score;
        Goal = goal;
        RankIdx = rankIdx;
    }
}

public enum ScoreKind { Distance, PushUp }
public struct Score 
{
    private double Value;
    private ScoreKind Kind;
    private Score(double value, ScoreKind kind) { Value = value; Kind = kind; }

    public double AsDouble { get => Value; set => Value = value; }
    public int    AsInt    { get => (int)Value; set => Value = (double)value; }

    public Meter  Meter  { get => AsDouble; set => AsDouble = value; }
    public Meter  KiloMeter  { get => Meter / 1000.0; set => Meter = value * 1000.0; }
    public PushUp PushUp { get => AsInt; set => AsInt = value; }

    public static Score NewMeter (Meter   meter) => new Score(meter, ScoreKind.Distance);
    public static Score NewKiloMeter(Meter kiloMeter) => new Score(kiloMeter * 1000, ScoreKind.Distance);
    public static Score NewPushUp(PushUp pushUp) => new Score(pushUp, ScoreKind.PushUp);

    public string DistanceString => KiloMeter <= 1.0 ? Meter + " m" : KiloMeter + " km";
    public string PushUpString => PushUp + " push up";

    public override string ToString()
    {
        switch (Kind)
        {
            case ScoreKind.Distance: return DistanceString;
            case ScoreKind.PushUp: return PushUpString;
        }
        throw new Exception("unknow " + Kind);
    }
}

public enum GoalKind 
{
    Run,
    PushUp,
}

/// <summary>
/// Description of a Goal
/// </summary>
public class Goal : PlatyAppComponent, IEnumerable<Rank>
{
    public GoalID        ID = GoalID.Empty;
    public GoalKind      Kind;
    public List<Rank>    Rank;
    public TimeSpan?     MaxTime;

    public string Description => Kind.ToString();

    public string KindImgPath 
    { 
        get 
        {
            switch (Kind)
            {
                case GoalKind.Run: return "https://media.tenor.com/mo6Te6bSxEcAAAAi/quby-run.gif"; // "run.png";
                case GoalKind.PushUp: return "https://c.tenor.com/NWooEQHLpTgAAAAC/tenor.gif"; // "push_up.png";
            }
            throw new NotImplementedException();
        } 
    }

    public Goal(GoalKind kind, List<Rank> rank, TimeSpan? maxTime = null) : this(GoalID.Empty, kind, rank, maxTime) { }
    public Goal(ChallengeID id, GoalKind kind, List<Rank> rank, TimeSpan? maxTime = null)
    {
        ID = id;
        Kind = kind;
        MaxTime = maxTime;
        Rank = rank;
    }

    public IEnumerator<Rank> GetEnumerator() => ((IEnumerable<Rank>)Rank).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Rank).GetEnumerator();

    public override string ToString()
    {
        var kind_str = "?";
        switch (Kind)
        {
            case GoalKind.Run: { kind_str = "run"; } break;
            case GoalKind.PushUp:  { kind_str = "push up"; } break;
        }
        return kind_str + " (" + String.Join(", ", Rank.Select(r => r.ToString())) + ")";
    }
}

public class Rank : PlatyAppComponent
{
    Score    ScoreToReach;
    XP       Reward;

    public Rank(Score scoreToReach, XP reward)
    {
        ScoreToReach = scoreToReach;
        Reward = reward;
    }

    public override string ToString() => "[" + ScoreToReach + " => " + Reward + "]";
}


public class User : PlatyAppComponent
{
    public static User Default { get; private set; } = new User();
    UserID ID = UserID.Empty;
    Pseudo Pseudo = Pseudo.Default;

    DateTime CreationData = DateTime.Now;
    DateTime Birthday = new DateTime(1000);

    XP XP = XP.Zero;

    public User() { }
    public User(Pseudo pseudo, DateTime creationData, DateTime birthday, XP xP) : this(UserID.Empty, pseudo, creationData, birthday, xP) { } 
    public User(UserID id, Pseudo pseudo, DateTime creationData, DateTime birthday, XP xP)
    {
        ID = id;
        XP = xP;
        Pseudo = pseudo;
        CreationData = creationData;
        Birthday = birthday;
    }

    public override string ToString() => Pseudo + "#" + ID;
}

public static class PseudoExtension 
{
    public static Pseudo ToPseudo(this string name) => new Pseudo(name);
    public static bool PseudoIsValid(this string name) => Pseudo.IsValid(name);
}

public record Pseudo
{
    public string Name { get; set; }

    public static Pseudo Default { get; private set; } = new("undefined");

    /// <summary>
    /// Stored in ascii order
    /// </summary>
    const string AllowedChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZ_abcdefghijklmnopqrstuvxwyz";

    public Pseudo(string name)
    {
        if (name.PseudoIsValid())
        {
            Name = name;
        }
        else 
        {
            Name = "John_Doe";
        }
    }

    public static bool IsValid(string name) => name.Length <= 16 && name.All(c => AllowedChar.Contains(c));
    public override string ToString() => Name;
}

public struct XP
{
    public static XP Zero { get; private set; } = new XP(0);
    public int Value;

    public XP(int value)
    {
        Value = value;
    }

    public override string ToString() 
    {
        if (Value <= 1000) { return Value + " xp"; }
        return Value / 1000.0 + "K xp";
    }

    public static XP operator+(XP left, XP right) => new XP(left.Value + right.Value);
    public static XP operator-(XP left, XP right) => new XP(left.Value - right.Value);
    public static XP operator -(XP left) => new XP(-left.Value);
    public static XP operator*(XP left, int right) => new XP(left.Value * right);
}

