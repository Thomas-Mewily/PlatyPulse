using PlatyPulseAPI.Value;

namespace PlatyPulseAPI.Data;

public record Rank(Score ScoreToReach, XP Reward)
{
    public Score ScoreToReach { get; set; } = ScoreToReach;
    public XP Reward { get; set; } = Reward;

    public Rank() : this(0.Meter(), 0.XP()) { }

    public override string ToString() => "[" + ScoreToReach + " => " + Reward + "]";
}