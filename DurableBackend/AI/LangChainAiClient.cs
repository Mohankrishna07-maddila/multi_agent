using LangChain.Providers;
using LangChain.Providers.Ollama;

namespace DurableBackend.AI;

public class LangChainAiClient : IAiClient
{
    private readonly OllamaChatModel _model;

    public LangChainAiClient()
    {
        try
        {
            Console.WriteLine("[LangChainAiClient] Initializing...");
            // Try string constructor
            var provider = new OllamaProvider(url: "http://localhost:11434");
            Console.WriteLine("[LangChainAiClient] Provider created.");
            _model = new OllamaChatModel(provider, "llama3.2:1b");
            Console.WriteLine("[LangChainAiClient] Model created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LangChainAiClient] Constructor Failed: {ex}");
            throw;
        }
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        if (_model == null) throw new InvalidOperationException("Model not initialized");
        var response = await _model.GenerateAsync($"You are a helpful AI assistant.\n\n{prompt}");
        return response.ToString();
    }
}
