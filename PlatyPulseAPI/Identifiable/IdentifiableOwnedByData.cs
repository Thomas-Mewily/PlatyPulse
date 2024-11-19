using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public class IdentifiableData : PlatyAppComponent
{
    public ID ID { get; set; } = ID.NewGuid();

    //public UserID OwnedByUser {  get => UserID[; set; }

    /// <summary>
    /// Upload the data to the server
    /// </summary>
    public virtual void ServerUpload() { "todo".Panic(); }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public virtual void ServerDownload() { "todo".Panic(); }

    public override string ToString() => $"{GetType().Name}#{ID}";
}


public class IdentifiableOwnedByData : IdentifiableData
{
    public ID OwnedByUserID { get; set; } = ID.Empty;

    [NotMapped] [JsonIgnore]
    public bool IsOnlyOwnedByAdmin => OwnedByUserID == ID.Empty;
}
