using LangChain.Providers;
using LangChain.Providers.Ollama;
using DurableBackend.AI.Graph;

namespace DurableBackend.AI.Graph;

public class ResponseNode
{
    private readonly OllamaChatModel _model;

    public ResponseNode()
    {
        var provider = new OllamaProvider();
        _model = new OllamaChatModel(provider, "llama3.2:1b");
    }

    public async Task<AgentState> Run(AgentState state)
    {
        var prompt = $"Based on this analysis, respond clearly:\n{state.Analysis}";
        var response = await _model.GenerateAsync(prompt);
        state.Response = response.ToString();
        return state;
    }
}
