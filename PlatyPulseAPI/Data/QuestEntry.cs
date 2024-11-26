using PlatyPulseAPI.Value;

namespace PlatyPulseAPI.Data;

/// <summary>
/// A participation for a person in an objectif
/// </summary>
public class QuestEntry : IdentifiableOwnedByData
{
    public UserID  UserID  { get; set; }
    public Score   Score   { get; set; }
    public Quest   Quest   { get; set; }
    public int     RankIdx { get; set; }

    public QuestEntry() : this(ChallengeID.Empty, UserID.Empty, 1.Meter(), new Quest(QuestKind.Run)) { }
    public QuestEntry(UserID user_id, Score score, Quest quest, int rankIdx = 0) : this(QuestEntryID.Empty, user_id, score, quest, rankIdx) { }
    public QuestEntry(ChallengeID id, UserID user_id, Score score, Quest quest, int rankIdx = 0)
    {
        ID = id;
        UserID = user_id;
        Score = score;
        Quest = quest;
        RankIdx = rankIdx;
    }
}
