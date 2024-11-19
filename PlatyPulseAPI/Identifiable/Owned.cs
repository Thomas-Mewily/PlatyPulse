using PlatyPulseAPI.Value;
namespace PlatyPulseAPI.Data;

public abstract class Owned<T> : IdentifiableOwnedByData where T : new()
{
    public T Value { get; set; } = new();

    public Owned() { }
    public Owned(T value) { Value = value; }

    public override string ToString() => Value!.ToString() + " : " + base.ToString();
}

public class OwnedEmail : Owned<Email> 
{
    public OwnedEmail() { }
    public OwnedEmail(Email value) { Value = value; }
}
public class OwnedPseudo : Owned<Pseudo> 
{
    public OwnedPseudo() { }
    public OwnedPseudo(Pseudo pseudo) { Value = pseudo; }
}

