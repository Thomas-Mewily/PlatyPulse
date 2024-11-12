using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

/// <summary>
/// A participation for a person in a challenge
/// </summary>
public class ChallengeEntry : IdentifiableData
{
    public User User { get; set; } = User.Default;
    public List<QuestEntry> Quest { get; set; } = [];

    public ChallengeEntry() { }
    public ChallengeEntry(User user, List<QuestEntry> quests_entries) : this(ChallengeID.NewGuid(), user, quests_entries) { }
    public ChallengeEntry(ChallengeID id, User user, List<QuestEntry> quest_entries)
    {
        ID = id;
        User = user;
        Quest = quest_entries;
    }
}