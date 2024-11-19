using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public class IdentifiableData : PlatyAppComponent
{
    public ID ID { get; set; } = ID.Empty;
    public ID OwnedByUserID { get; set; } = ID.Empty;

    [JsonIgnore]
    public bool IsOnlyOwnedByAdmin => OwnedByUserID == ID.Empty;

    //public UserID OwnedByUser {  get => UserID[; set; }

    /// <summary>
    /// Upload the data to the server
    /// </summary>
#pragma warning disable CA1822 // Marquer les membres comme étant static
    public virtual void ServerUpload() { "todo".Panic(); }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public virtual void ServerDownload() { "todo".Panic(); }
#pragma warning restore CA1822 // Marquer les membres comme étant static

    public override string ToString() => $"{GetType().Name}#{ID}";
}
