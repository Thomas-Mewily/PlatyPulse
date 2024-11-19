using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public enum ChallengePeriod
{
    Past,
    Active,
    Futur,
}

public class Challenge : IdentifiableData
{
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
            if (IsPast) { return ChallengePeriod.Past; }
            if (IsFutur) { return ChallengePeriod.Futur; }
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
    public Challenge(List<Quest> quests) { Quests = quests;  }
    public static Challenge Daily(List<Quest> objectifs) => new(objectifs);

    public override string ToString()
    {
        var time_str = "";
        switch (TimePeriod)
        {
            case ChallengePeriod.Active: { time_str = "current"; } break;
            case ChallengePeriod.Futur: { time_str = "futur"; } break;
            case ChallengePeriod.Past: { time_str = "past"; } break;
        }

        var duration_str = Duration.TotalHours + " h";
        if (Duration == TimeSpan.FromDays(1)) { duration_str = "daily"; }
        if (Duration == TimeSpan.FromDays(7)) { duration_str = "weekly"; }

        return time_str + " " + duration_str + " challenge consist of " + string.Join(" and ", Quests.Select(x => x.ToString()));
    }
}
