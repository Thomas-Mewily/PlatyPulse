using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public enum ChallengePeriod
{
    Past,
    Active,
    Futur,
}

public class Challenge : IdentifiableByID
{
    /// ================= Fields =========

    public DateTime Begin { get; set; } = DateTime.Now;
    public TimeSpan Duration { get; set; } = TimeSpan.FromDays(1);

    public List<Quest> Quests { get; set; } = [];

    public override void ForceUpdateFrom(IdentifiableByID other)
    {
        var c = (other as Challenge).Unwrap();
        Begin = c.Begin;
        Duration = c.Duration;
        Quests = c.Quests;
    }

    /// ================= Rest =========
    [NotMapped] [JsonIgnore]
    public DateTime End => Begin + Duration;
    [NotMapped] [JsonIgnore]
    public TimeSpan TimeRemaning => End - CurrentTime;

    [NotMapped] [JsonIgnore]
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

    [NotMapped] [JsonIgnore]
    public bool IsActive => CurrentTime >= Begin && CurrentTime < End;
    [NotMapped] [JsonIgnore]
    public bool IsPast => End < CurrentTime;
    [NotMapped] [JsonIgnore]
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

    public override async Task ServerUpdate() => await _ServerUpdate<Challenge>(this);
    public override async Task ServerDownload() => await _ServerDownload<Challenge>();
}
