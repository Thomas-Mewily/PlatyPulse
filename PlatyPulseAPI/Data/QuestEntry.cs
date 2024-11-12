namespace PlatyPulseAPI.Data;

/// <summary>
/// A participation for a person in an objectif
/// </summary>
public class QuestEntry : IdentifiableData
{
    public User  User    { get; set; }
    public Score Score   { get; set; }
    public Quest Quest   { get; set; }
    public int   RankIdx { get; set; }

    public QuestEntry() : this(ChallengeID.Empty, User.Default, 1.Meter(), new Quest(QuestKind.Run)) { }
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
