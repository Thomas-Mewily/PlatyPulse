using PlatyPulseAPI.Data;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Value;

public static class MailExtension
{
    public static OwnedEmail ToOwnedMail(this string email) => email.ToEmail().ToOwned();
    public static OwnedEmail ToOwned(this Email email) => new(email);

    public static Email ToEmail(this string email) => new(email);
}

public record Email
{
    public string Address { get; set; }

    public Email() { Address = ""; }
    public Email(string address) { Address = address; }

    [NotMapped] [JsonIgnore]
    public bool IsValid 
    { 
        get 
        { 
            if (Address.Length == 0) { return false; }
            try
            {
                var emailAddress = new MailAddress(Address);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public override string ToString() => Address;
}

