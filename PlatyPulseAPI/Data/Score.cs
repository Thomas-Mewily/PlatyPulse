namespace PlatyPulseAPI.Data;

public enum ScoreKind { Distance, PushUp }
public struct Score
{
    public double Value { get; set; }
    public ScoreKind Kind { get; set; }

    public Score() : this(0, ScoreKind.Distance) { }
    private Score(double value, ScoreKind kind) { Value = value; Kind = kind; }

    public double AsDouble { readonly get => Value; set => Value = value; }
    public int AsInt { readonly get => (int)Value; set => Value = (double)value; }

    public Meter Meter { readonly get => AsDouble; set => AsDouble = value; }
    public Meter KiloMeter { readonly get => Meter / 1000.0; set => Meter = value * 1000.0; }
    public PushUp PushUp { readonly get => AsInt; set => AsInt = value; }

    public static Score NewMeter(Meter meter) => new(meter, ScoreKind.Distance);
    public static Score NewKiloMeter(Meter kiloMeter) => new(kiloMeter * 1000, ScoreKind.Distance);
    public static Score NewPushUp(PushUp pushUp) => new(pushUp, ScoreKind.PushUp);

    public readonly string DistanceString => KiloMeter <= 1.0 ? Meter + " m" : KiloMeter + " km";
    public readonly string PushUpString => PushUp + " push up";

    public override readonly string ToString()
    {
        return Kind switch
        {
            ScoreKind.Distance => DistanceString,
            ScoreKind.PushUp => PushUpString,
            _ => throw new Exception("unknow " + Kind),
        };
    }
}