using System.Text.Json.Serialization;
namespace PlatyPulseAPI.Data;

public enum QuestKind
{
    Run,
    PushUp,
}

/// <summary>
/// Description of a Quest
/// </summary>
public class Quest : IdentifiableData
{
    public QuestKind Kind { get; set; }
    public List<Rank> Rank { get; set; }
    public TimeSpan? MaxTime { get; set; }

    [JsonIgnore]
    public string Description => Kind.ToString();

    [JsonIgnore]
    public string KindImgPath
    {
        get
        {
            return Kind switch
            {
                QuestKind.Run => "https://media.tenor.com/mo6Te6bSxEcAAAAi/quby-run.gif",// "run.png";
                QuestKind.PushUp => "https://c.tenor.com/NWooEQHLpTgAAAAC/tenor.gif",// "push_up.png";
                _ => throw new NotImplementedException(),
            };
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
            case QuestKind.PushUp: { kind_str = "push up"; } break;
        }
        return kind_str + " (" + string.Join(", ", Rank.Select(r => r.ToString())) + ")";
    }
}