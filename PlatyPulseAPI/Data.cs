using System.Collections;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace PlatyPulseAPI;

public abstract class Data : PlatyAppComponent
{
    public ID ID { get; set; }

    /// <summary>
    /// Upload the data to the server
    /// </summary>
    public void ServerUpload() { "todo".Panic(); }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public void ServerDownload() { "todo".Panic(); }
}

public enum ChallengePeriod 
{
    Past,
    Active,
    Futur,
}

public class Challenge : Data 
{
    public static Challenge Default => new();

    /// ================= Fields =========

    public DateTime Begin { get; set; } = DateTime.Now;
    public TimeSpan Duration { get; set; } = TimeSpan.FromDays(1);

    public List<Quest> Quests { get; set; } = [];

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
    public static Challenge Daily(List<Quest> objectifs) { var c = new Challenge(); c.Quests = objectifs; return c; }

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

        return time_str + " " + duration_str + " challenge consist of " + string.Join(" and ", Quests.Select(x => x.ToString()));
    }
}

/// <summary>
/// A participation for a person in a challenge
/// </summary>
public class ChallengeEntry : Data
{
    public User              User { get; set; } = User.Default;
    public List<QuestEntry>  Quest { get; set; } = [];

    public ChallengeEntry() { }
    public ChallengeEntry(User user, List<QuestEntry> quests_entries) : this(ChallengeID.NewGuid(), user, quests_entries) { }
    public ChallengeEntry(ChallengeID id, User user, List<QuestEntry> quest_entries)
    {
        ID = id;
        User = user;
        Quest = quest_entries;
    }
}

/// <summary>
/// A participation for a person in an objectif
/// </summary>
public class QuestEntry : Data
{
    public User        User    { get; set; }
    public Score       Score   { get; set; }
    public Quest       Quest   { get; set; }
    public int         RankIdx { get; set; }

    public QuestEntry() : this(ID.Empty, User.Default, 1.Meter(), new Quest(QuestKind.Run)) { }
    public QuestEntry(User user, Score score, Quest quest, int rankIdx = 0) : this(QuestEntryID.Empty, user, score, quest, rankIdx) { }
    public QuestEntry(ChallengeID id, User user, Score score, Quest quest, int rankIdx = 0)
    {
        ID = id;
        User = user;
        Score = score;
        Quest = quest;
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

public enum QuestKind 
{
    Run,
    PushUp,
}

/// <summary>
/// Description of a Quest
/// </summary>
public class Quest : Data
{
    public QuestKind    Kind    { get; set; }
    public List<Rank>  Rank    { get; set; }
    public TimeSpan?   MaxTime { get; set; }

    [JsonIgnore]
    public string Description => Kind.ToString();

    [JsonIgnore]
    public string KindImgPath 
    { 
        get 
        {
            switch (Kind)
            {
                case QuestKind.Run: return "https://media.tenor.com/mo6Te6bSxEcAAAAi/quby-run.gif"; // "run.png";
                case QuestKind.PushUp: return "https://c.tenor.com/NWooEQHLpTgAAAAC/tenor.gif"; // "push_up.png";
            }
            throw new NotImplementedException();
        } 
    }

    public Quest() : this(QuestKind.Run, []) { }
    public Quest(QuestKind kind, TimeSpan? maxTime = null) : this(kind, [], maxTime) { }
    public Quest(QuestKind kind, List<Rank> rank, TimeSpan? maxTime = null) : this(QuestID.Empty, kind, rank, maxTime) { }
    public Quest(ChallengeID id, QuestKind kind, List<Rank> rank, TimeSpan? maxTime = null)
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
            case QuestKind.Run: { kind_str = "run"; } break;
            case QuestKind.PushUp:  { kind_str = "push up"; } break;
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


public class User : Data
{
    public static User Default => new User();

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
