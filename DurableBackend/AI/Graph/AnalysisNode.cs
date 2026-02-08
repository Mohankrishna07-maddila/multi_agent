using LangChain.Providers;
using LangChain.Providers.Ollama;
using DurableBackend.AI.Graph;

namespace DurableBackend.AI.Graph;

public class AnalysisNode
{
    private readonly OllamaChatModel? _model;
    private readonly bool _useMock;

    public AnalysisNode()
    {
        // Try to use Ollama for local development
        try
        {
            var provider = new OllamaProvider();
            _model = new OllamaChatModel(provider, "llama3.2:1b");
            _useMock = false;
        }
        catch
        {
            // Fall back to mock if Ollama is not available
            _useMock = true;
        }
    }

    public async Task<AgentState> Run(AgentState state)
    {
        Console.WriteLine($"[AnalysisNode] Running with _useMock={_useMock}");
        
        if (_useMock || _model == null)
        {
            // Mock response for testing/Azure deployment
            await Task.Delay(500); // Simulate processing
            state.Analysis = $"[Mock Analysis] Analyzed input: '{state.Input}'. " +
                           $"This appears to be a user query that requires a thoughtful response. " +
                           $"Key topics identified: general inquiry, conversational AI.";
        }
        else
        {
            Console.WriteLine("[AnalysisNode] Generating response with Ollama...");
            var prompt = $"Analyze the following input:\n{state.Input}";
            var response = await _model.GenerateAsync(prompt);
            state.Analysis = response.ToString();
            Console.WriteLine("[AnalysisNode] specific_response_generated");
        }
        
        return state;
    }
}
