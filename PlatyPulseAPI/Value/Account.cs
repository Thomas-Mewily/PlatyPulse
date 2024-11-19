using PlatyPulseAPI.Data;

namespace PlatyPulseAPI.Value;

public class Account : IdentifiableData
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHashed { get; set; } = string.Empty;
}

public class AccountDTO
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}