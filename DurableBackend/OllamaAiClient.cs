using System.Net.Http.Json;
using System.Text.Json;

namespace DurableBackend.AI;

public class OllamaAiClient : IAiClient
{
    private readonly HttpClient _http;

    public OllamaAiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        var request = new
        {
            model = "llama3.2:1b",
            prompt = prompt,
            stream = false
        };

        var response = await _http.PostAsJsonAsync(
            "http://localhost:11434/api/generate",
            request);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        return json.GetProperty("response").GetString()!;
    }
}
