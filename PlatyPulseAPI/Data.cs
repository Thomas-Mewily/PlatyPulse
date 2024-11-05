using System.Collections;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace PlatyPulseAPI;

public enum ChallengePeriod 
{
    Past,
    Active,
    Futur,
}

public class Challenge : PlatyAppComponent, IData
{
    public static Challenge Default => new();

    /// ================= Fields =========
    public ChallengeID ID { get; set; } = ChallengeID.Empty;

    public DateTime Begin { get; set; } = DateTime.Now;
    public TimeSpan Duration { get; set; } = TimeSpan.FromDays(1);

    public List<Goal> Goals { get; set; } = [];

    /// ================= Properties =========
    public DateTime End => Begin + Duration;
    public TimeSpan TimeRemaning => End - CurrentTime;

    [JsonIgnore]
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

    [JsonIgnore]
    public bool IsActive => CurrentTime >= Begin && CurrentTime < End;
    [JsonIgnore]
    public bool IsPast => End < CurrentTime;
    [JsonIgnore]
    public bool IsFutur => Begin >= CurrentTime;

    public Challenge() { }
    public static Challenge Daily(List<Goal> objectifs) { var c = new Challenge(); c.Goals = objectifs; return c; }

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
public class ChallengeEntry : PlatyAppComponent, IData
{
    public ChallengeEntryID ID { get; set; } = ChallengeID.Empty;
    public User             User { get; set; } = User.Default;
    public List<GoalEntry>  Goals { get; set; } = [];

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
public class GoalEntry : PlatyAppComponent, IData
{
    public GoalEntryID ID { get; set;}
    public User        User { get; set; }
    public Score       Score { get; set; }
    public Goal        Goal { get; set; }
    public int         RankIdx { get; set; }

    public GoalEntry() : this(ID.Empty, User.Default, 1.Meter(), new Goal(GoalKind.Run)) { }
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
    public double Value { get; set; }
    public ScoreKind Kind { get; set; }

    public Score() : this(0, ScoreKind.Distance) { }
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
public class Goal : PlatyAppComponent, IData
{
    public GoalID      ID      { get; set; } = GoalID.Empty;
    public GoalKind    Kind    { get; set; }
    public List<Rank>  Rank    { get; set; }
    public TimeSpan?   MaxTime { get; set; }

    public string Description => Kind.ToString();

    [JsonIgnore]
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

    public Goal() : this(GoalKind.Run, []) { }
    public Goal(GoalKind kind, TimeSpan? maxTime = null) : this(kind, [], maxTime) { }
    public Goal(GoalKind kind, List<Rank> rank, TimeSpan? maxTime = null) : this(GoalID.Empty, kind, rank, maxTime) { }
    public Goal(ChallengeID id, GoalKind kind, List<Rank> rank, TimeSpan? maxTime = null)
    {
        ID = id;
        Kind = kind;
        MaxTime = maxTime;
        Rank = rank;
    }

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
    public Score ScoreToReach { get; set; }
    public XP Reward { get; set; }

    public Rank() : this(0.Meter(), 0.XP()) { }
    public Rank(Score scoreToReach, XP reward)
    {
        ScoreToReach = scoreToReach;
        Reward = reward;
    }

    public override string ToString() => "[" + ScoreToReach + " => " + Reward + "]";
}


public class User : PlatyAppComponent, IData
{
    public static User Default => new User();

    public UserID ID { get; set; } = UserID.Empty;
    public Pseudo Pseudo { get; set; } = Pseudo.Default;

    public DateTime CreationData { get; set; } = DateTime.Now;
    public DateTime Birthday { get; set; } = new DateTime(1000);

    public XP XP { get; set; } = XP.Zero;

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

    public static Pseudo Default => new("undefined");

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
    public int Value { get; set; }

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
    public static XP operator-(XP left) => new XP(-left.Value);
    public static XP operator*(XP left, int right) => new XP(left.Value * right);
}


public class Test
{
    public string Nom;
    public string Prenom { get; set; }

    public Test(string nom, string prenom)
    {
        Nom = nom;
        Prenom = prenom;
    }

}
