namespace PlatyPulseAPI.Data;

public class User : IdentifiableData
{
    public static User Default => new();

    public Pseudo Pseudo { get; set; } = Pseudo.Default;

    public DateTime CreationData { get; set; } = DateTime.Now;
    public DateTime Birthday { get; set; } = new DateTime(1000);

    public XP XP { get; set; } = XP.Zero;

    public User() { }
    public User(Pseudo pseudo, DateTime creationData, DateTime birthday, XP xP) : this(UserID.Empty, pseudo, creationData, birthday, xP) { }
    public User(UserID id, Pseudo pseudo, DateTime creationData, DateTime birthday, XP xP)
    {
        ID = id;
        XP = xP;
        Pseudo = pseudo;
        CreationData = creationData;
        Birthday = birthday;
    }

    public override string ToString() => Pseudo + "#" + ID;
}