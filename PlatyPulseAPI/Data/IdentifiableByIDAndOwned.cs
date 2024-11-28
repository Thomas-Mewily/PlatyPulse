using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public abstract class IdentifiableByID : PlatyAppComponent
{
    public ID ID { get; set; } = ID.NewGuid();
    public ID OwnedByUserID { get; set; } = ID.Empty;

    //public UserID OwnedByUser {  get => UserID[; set; }

    public void GenerateNewID() { while (ID == ID.Empty) { ID = Guid.NewGuid(); } }

    public bool CanBeEditedBy(User u) => u.IsAdmin || _CanBeEditedBy(u);
    protected virtual bool _CanBeEditedBy(User u) => u.ID == OwnedByUserID;
    [NotMapped]
    [JsonIgnore]
    public bool IsOnlyOwnedByAdmin => OwnedByUserID == ID.Empty;


    /// <summary>
    /// Upload the data to the server
    /// </summary>
    public async Task ServerUpdate() 
    {
        await App.DbPutAsync(GetType().Name + "/" + ID.ToString(), this);
    }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public async Task ServerDownload() 
    {
        var downloaded = await App.DbGetAsync<IdentifiableByID>(GetType().Name + "/" + ID.ToString());
        ForceUpdateAllFrom(downloaded);
    }

    public override string ToString() => $"{GetType().Name}#{ID}";

    public virtual void ForceUpdateAllFrom(IdentifiableByID other) => ForceUpdateFrom(other);

    public abstract void ForceUpdateFrom(IdentifiableByID other);
    public void UpdateFrom(IdentifiableByID other, User askedBy) 
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