namespace PlatyPulseAPI.Data;

public class Rank(Score scoreToReach, XP reward) : PlatyAppComponent
{
    public Score ScoreToReach { get; set; } = scoreToReach;
    public XP Reward { get; set; } = reward;

    public Rank() : this(0.Meter(), 0.XP()) { }

    public override string ToString() => "[" + ScoreToReach + " => " + Reward + "]";
}