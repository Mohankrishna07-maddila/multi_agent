using LangChain.Providers;
using LangChain.Providers.Ollama;
using DurableBackend.AI.Graph;

namespace DurableBackend.AI.Graph;

public class ResponseNode
{
    private readonly OllamaChatModel? _model;
    private readonly bool _useMock;

    public ResponseNode()
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
        Console.WriteLine($"[ResponseNode] Running with _useMock={_useMock}");

        if (_useMock || _model == null)
        {
            // Mock response for testing/Azure deployment
            await Task.Delay(500); // Simulate processing
            state.Response = $"Hello! I've processed your input: '{state.Input}'. " +
                           $"Based on the analysis, here's my response: This is a mock AI response " +
                           $"generated for testing purposes. In production, this would be powered by " +
                           $"Ollama or another AI service. Your query has been acknowledged!";
        }
        else
        {
            Console.WriteLine("[ResponseNode] Generating response with Ollama...");
            var prompt = $"Based on this analysis, respond clearly:\n{state.Analysis}";
            var response = await _model.GenerateAsync(prompt);
            state.Response = response.ToString();
            Console.WriteLine("[ResponseNode] specific_response_generated");
        }
        
        return state;
    }
}
