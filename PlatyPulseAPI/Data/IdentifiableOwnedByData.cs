using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public abstract class IdentifiableData : PlatyAppComponent
{
    public ID ID { get; set; } = ID.NewGuid();

    //public UserID OwnedByUser {  get => UserID[; set; }

    public void GenerateNewID() { while (ID == ID.Empty) { ID = Guid.NewGuid(); } }

    public bool CanBeEditedBy(User u) => u.IsAdmin || _CanBeEditedBy(u);
    protected virtual bool _CanBeEditedBy(User u) => false;

    /// <summary>
    /// Upload the data to the server
    /// </summary>
    public async Task ServerUpdate() 
    {
        
    }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public async Task ServerDownload() 
    { 
        
    }

    public override string ToString() => $"{GetType().Name}#{ID}";

    public abstract void ForceUpdateFrom(IdentifiableData other);
    public void UpdateFrom(IdentifiableData other, User askedBy) 
    { 
        if (!CanBeEditedBy(askedBy)) 
        {
            throw new Exception(GetType().Name + "#" + ID + " can't be updated by " + askedBy);
        }
        ForceUpdateFrom(other);
    }

    [NotMapped]
    [JsonIgnore]
    public bool IsPublicData => !IsPrivateData;

    [NotMapped]
    [JsonIgnore]
    public virtual bool IsPrivateData => false;
}


public abstract class IdentifiableOwnedByData : IdentifiableData
{
    public ID OwnedByUserID { get; set; } = ID.Empty;

    [NotMapped] [JsonIgnore]
    public bool IsOnlyOwnedByAdmin => OwnedByUserID == ID.Empty;

    protected override bool _CanBeEditedBy(User u) => u.ID == OwnedByUserID;
}
