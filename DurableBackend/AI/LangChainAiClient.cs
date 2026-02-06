using LangChain;
using LangChain.Providers.Ollama;
using LangChain.Prompts;

namespace DurableBackend.AI;

public class LangChainAiClient : IAiClient
{
    private readonly OllamaChatModel _model;

    public LangChainAiClient()
    {
        _model = new OllamaChatModel(
            model: "llama3.2:1b",
            endpoint: "http://localhost:11434");
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        var template = new PromptTemplate(
            "You are a helpful AI assistant.\n\n{input}");

        var chain = template | _model;

        var result = await chain.RunAsync(new { input = prompt });

        return result;
    }
}
