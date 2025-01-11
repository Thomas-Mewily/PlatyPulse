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

    /// <summary>
    /// Create a new object and send it to the server
    /// </summary>
    public abstract Task ServerCreate();
    protected async Task _ServerCreate<T>(T value) where T : IdentifiableByID
    {
        Console.WriteLine("Creating " + typeof(T).Name + " to the server");
        await App.DbPostAsync<T, T>(GetType().Name + "/", value);

        /*
        try
        {
            // ActionResult<T>
            // Idk what the second generic type is so I'm just going to use int and try catch x)
            
            // I don't want to install it
            //await App.DbPostAsync<T, ActionResult<T>>(GetType().Name + "/" + ID.ToString(), value);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }*/
    }

    /// <summary>
    /// Upload the data to the server
    /// </summary>
    public abstract Task ServerUpdate();
    protected async Task _ServerUpdate<T>(T value) where T : IdentifiableByID
    {
        Console.WriteLine("Uploading " + typeof(T).Name + " to the server");
        await App.DbPutAsync<T>(GetType().Name + "/" + ID.ToString(), value);
    }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public abstract Task ServerDownload();
    protected async Task _ServerDownload<T>() where T : IdentifiableByID
    {
        Console.WriteLine("Downloading " + typeof(T).Name + " from the server");
        var downloaded = await App.DbGetAsync<T>(GetType().Name + "/" + ID.ToString());
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