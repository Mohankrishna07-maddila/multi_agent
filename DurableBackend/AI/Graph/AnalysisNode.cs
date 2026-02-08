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
        var ollamaEndpoint = Environment.GetEnvironmentVariable("OLLAMA_ENDPOINT");
        Console.WriteLine($"[AnalysisNode] OLLAMA_ENDPOINT value: '{ollamaEndpoint}'");
        
        // If running locally (no endpoint set), FORCE real AI
        if (string.IsNullOrEmpty(ollamaEndpoint))
        {
            Console.WriteLine("[AnalysisNode] Local environment detected. connecting to local Ollama...");
            try 
            {
                var provider = new OllamaProvider(); // http://localhost:11434
                _model = new OllamaChatModel(provider, "llama3.2:1b");
                _useMock = false;
                Console.WriteLine("[AnalysisNode] Connected to local Ollama.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnalysisNode] FATAL: Could not connect to local Ollama. {ex.Message}");
                throw; // Throw to stop execution and show error, don't use mock locally
            }
        }
        else
        {
            // Azure/Cloud Environment - Default to mock for now
            // Future: Implement Azure OpenAI or configured Ollama
            Console.WriteLine("[AnalysisNode] Cloud environment detected (OLLAMA_ENDPOINT set). Defaulting to mock.");
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
