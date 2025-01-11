using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

/// <summary>
/// A participation for a person in a challenge
/// </summary>
public class ChallengeEntry : IdentifiableByID
{
    /// ================= Fields ========= <summary>
    public List<QuestEntry> Quest { get; set; } = [];

    public override void ForceUpdateFrom(IdentifiableByID other)
    {
        var c = (other as ChallengeEntry).Unwrap();
        c.Quest = Quest;
    }

    /// ================= Rest =========

    [NotMapped]
    [JsonIgnore]
    public UserID UserID { get => OwnedByUserID; set => OwnedByUserID = value; }

    public ChallengeEntry() { }
    public ChallengeEntry(UserID user_id, List<QuestEntry> quests_entries) : this(ChallengeID.NewGuid(), user_id, quests_entries) { }
    public ChallengeEntry(ChallengeID id, UserID user_id, List<QuestEntry> quest_entries)
    {
        ID = id;
        UserID = user_id;
        Quest = quest_entries;
    }

    public override async Task ServerCreate() => await _ServerCreate<ChallengeEntry>(this);
    public override async Task ServerUpdate() => await _ServerUpdate<ChallengeEntry>(this);
    public override async Task ServerDownload() => await _ServerDownload<ChallengeEntry>();
}