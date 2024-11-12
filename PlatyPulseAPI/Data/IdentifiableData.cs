using System.Collections;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace PlatyPulseAPI.Data;

public abstract class IdentifiableData : PlatyAppComponent
{
    public ID ID { get; set; }

    /// <summary>
    /// Upload the data to the server
    /// </summary>
#pragma warning disable CA1822 // Marquer les membres comme étant static
    public void ServerUpload() { "todo".Panic(); }
    /// <summary>
    /// Download the data from the server
    /// </summary>
    public void ServerDownload() { "todo".Panic(); }
#pragma warning restore CA1822 // Marquer les membres comme étant static
}
