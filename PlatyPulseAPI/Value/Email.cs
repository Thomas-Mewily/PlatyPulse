using PlatyPulseAPI.Data;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Value;

public static class MailExtension
{
    /*
    public static OwnedEmail ToOwnedMail(this string email, UserID by) => email.ToEmail().ToOwned(by);
    public static OwnedEmail ToOwned(this Email email, UserID by) => new(email, by);
    */

    public static Email ToEmail(this string email) => new(email);
}

public record Email
{
    public string Address { get; set; }

    public Email() { Address = ""; }
    public Email(string address) 
    { 
        Address = address;
    }

    public Email Check() 
    {
        if (!IsValid) { throw new Exception("Invalid email " + Address);  }
        return this;
    }

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

public class EmailJsonConverter : JsonConverter<Email>
{
    public override Email Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String) { return reader.GetString().Unwrap().ToEmail(); }
        throw new JsonException("Invalid JSON format for Email.");
    }

    public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Address);
    }
}