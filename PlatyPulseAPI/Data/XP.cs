namespace PlatyPulseAPI.Data;

public struct XP(int value)
{
    public static XP Zero { get; private set; } = new XP(0);
    public int Value { get; set; } = value;

    public override readonly string ToString()
    {
        if (Value <= 1000) { return Value + " xp"; }
        return Value / 1000.0 + "K xp";
    }

    public static XP operator +(XP left, XP right) => new(left.Value + right.Value);
    public static XP operator -(XP left, XP right) => new(left.Value - right.Value);
    public static XP operator -(XP left) => new(-left.Value);
    public static XP operator *(XP left, int right) => new(left.Value * right);
}