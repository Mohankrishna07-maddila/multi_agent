using LangChain.Providers;
using LangChain.Providers.Ollama;

namespace DurableBackend.AI;

public class LangChainAiClient : IAiClient
{
    private readonly OllamaChatModel _model;

    public LangChainAiClient()
    {
        var provider = new OllamaProvider();
        _model = new OllamaChatModel(provider, "llama3.2:1b");
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        var response = await _model.GenerateAsync($"You are a helpful AI assistant.\n\n{prompt}");
        return response.ToString();
    }
}
