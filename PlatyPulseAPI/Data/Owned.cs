using PlatyPulseAPI.Value;
namespace PlatyPulseAPI.Data;

public abstract class Owned<T> : IdentifiableOwnedByData where T : new()
{
    public T Value { get; set; } = new();

    public Owned() { }
    public Owned(T value, UserID by) { Value = value; OwnedByUserID = by; }

    public override string ToString() => Value!.ToString() + " : " + base.ToString();
}

/*
public class OwnedEmail : Owned<Email> 
{
    public OwnedEmail() { }
    public OwnedEmail(Email value, UserID by) : base(value, by) { }
}
public class OwnedPseudo : Owned<Pseudo> 
{
    public OwnedPseudo() { }
    public OwnedPseudo(Pseudo value, UserID by) : base(value, by) { }
}
*/