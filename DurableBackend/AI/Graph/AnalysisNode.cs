using LangChain.Providers;
using LangChain.Providers.Ollama;
using DurableBackend.AI.Graph;

namespace DurableBackend.AI.Graph;

public class AnalysisNode
{
    private readonly OllamaChatModel _model;

    public AnalysisNode()
    {
        var provider = new OllamaProvider();
        _model = new OllamaChatModel(provider, "llama3.2:1b");
    }

    public async Task<AgentState> Run(AgentState state)
    {
        var prompt = $"Analyze the following input:\n{state.Input}";
        var response = await _model.GenerateAsync(prompt);
        state.Analysis = response.ToString();
        return state;
    }
}
