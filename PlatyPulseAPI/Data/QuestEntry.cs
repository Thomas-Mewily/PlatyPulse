using PlatyPulseAPI.Value;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

/// <summary>
/// A participation for a person in an objectif
/// </summary>
public class QuestEntry : IdentifiableByID
{
    /// ================= Fields =========
    public Score   Score   { get; set; }
    public Quest   Quest   { get; set; }

    public override void ForceUpdateFrom(IdentifiableByID other)
    {
        var q   = (other as QuestEntry).Unwrap();
        Score   = q.Score;
        Quest   = q.Quest;
    }

    /// ================= Rest =========
    [NotMapped]
    [JsonIgnore]
    public UserID UserID { get => OwnedByUserID; set => OwnedByUserID = value; }

    [NotMapped]
    [JsonIgnore]
    public int RankIdx 
    {
        get => Quest.GetRankIdx(Score);
        set => Score = Quest.MinimumScoreToReachRankIdx(value);
    }
    public Rank CurrentRank => Quest.Rank[RankIdx];

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

    public override async Task ServerCreate() => await _ServerCreate<QuestEntry>(this);
    public override async Task ServerUpdate() => await _ServerUpdate<QuestEntry>(this);
    public override async Task ServerDownload() => await _ServerDownload<QuestEntry>();
}
