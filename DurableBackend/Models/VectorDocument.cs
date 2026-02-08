using Newtonsoft.Json;

namespace DurableBackend.Models;

public class VectorDocument
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [JsonProperty("metadata")]
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    [JsonProperty("vector")]
    public float[] Vector { get; set; } = Array.Empty<float>();

    [JsonProperty("sessionId")] // Optional: to scope to session if needed
    public string SessionId { get; set; } = string.Empty;
}
