using Newtonsoft.Json;

namespace DurableBackend.Models;

public class ConversationRecord
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("sessionId")]
    public string SessionId { get; set; } = string.Empty;

    [JsonProperty("userInput")]
    public string UserInput { get; set; } = string.Empty;

    [JsonProperty("analysis")]
    public string Analysis { get; set; } = string.Empty;

    [JsonProperty("response")]
    public string Response { get; set; } = string.Empty;

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
