using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public enum Role 
{ 
    Consumer,
    Admin,
}

public class User : IdentifiableData
{
    public Pseudo Pseudo { get; set; } = Pseudo.Default;
    public Role Role { get; set; } = Role.Consumer;

    [JsonIgnore]
    public bool IsAdmin => Role == Role.Admin;

    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime Birthday { get; set; } = new DateTime(1000);

    public XP XP { get; set; } = XP.Zero;

    public User() { }
    public User(UserID id) : this() { ID = id; }
    public User(Pseudo pseudo, Role role) : this(pseudo, role, DateTime.Now, DateTime.Now, 0.XP()) { }
    public User(Pseudo pseudo, Role role, DateTime creationData, DateTime birthday, XP xP) : this(UserID.Empty, pseudo, role, creationData, birthday, xP) { }
    public User(UserID id, Pseudo pseudo, Role role, DateTime creationData, DateTime birthday, XP xP)
    {
        ID = id;
        XP = xP;
        Pseudo = pseudo;
        CreationDate = creationData;
        Birthday = birthday;
    }

    public override string ToString() => Pseudo + "#" + ID;
    public static User TestDefault => new();
    public static User TestDefaultAdmin = new("admin".ToPseudo(), Role.Admin);
}