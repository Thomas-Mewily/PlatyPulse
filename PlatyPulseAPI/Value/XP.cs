﻿namespace PlatyPulseAPI.Value;

public record XP(int Value)
{
    public static XP Zero { get; private set; } = new XP(0);
    public int Value { get; set; } = Value;

    public override string ToString()
    {
        if (Value <= 1000) { return Value + " xp"; }
        return Value / 1000.0 + "K xp";
    }

    public static XP operator +(XP left, XP right) => new(left.Value + right.Value);
    public static XP operator -(XP left, XP right) => new(left.Value - right.Value);
    public static XP operator -(XP left) => new(-left.Value);
    public static XP operator *(XP left, int right) => new(left.Value * right);
}