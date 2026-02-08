using LangChain.Providers;
using LangChain.Providers.Ollama;
using DurableBackend.Services;
using DurableBackend.Models;
using System.Text;

namespace DurableBackend.AI.Graph;

public class AnalysisNode
{
    private readonly OllamaChatModel? _model;
    private readonly OllamaEmbeddingModel? _embeddingModel;
    private readonly bool _useMock;
    private readonly CosmosDbService _cosmosService;

    public AnalysisNode(CosmosDbService cosmosService)
    {
        _cosmosService = cosmosService;
        var ollamaEndpoint = Environment.GetEnvironmentVariable("OLLAMA_ENDPOINT");
        Console.WriteLine($"[AnalysisNode] OLLAMA_ENDPOINT value: '{ollamaEndpoint}'");
        
        if (string.IsNullOrEmpty(ollamaEndpoint))
        {
            Console.WriteLine("[AnalysisNode] Local environment detected. connecting to local Ollama...");
            try 
            {
                var provider = new OllamaProvider(url: "http://localhost:11434");
                _model = new OllamaChatModel(provider, "llama3.2:1b");
                _embeddingModel = new OllamaEmbeddingModel(provider, "all-minilm"); // Ensure this model is pulled!
                _useMock = false;
                Console.WriteLine("[AnalysisNode] Connected to local Ollama.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnalysisNode] FATAL: Could not connect to local Ollama. {ex.Message}");
                throw;
            }
        }
        else
        {
            Console.WriteLine("[AnalysisNode] Cloud environment detected. Defaulting to mock.");
            _useMock = true;
        }
    }

    public async Task<AgentState> Run(AgentState state)
    {
        Console.WriteLine($"[AnalysisNode] Running with _useMock={_useMock}");
        
        if (_useMock || _model == null)
        {
            await Task.Delay(500); 
            state.Analysis = $"[Mock Analysis] Analyzed input: '{state.Input}'.";
        }
        else
        {
            Console.WriteLine("[AnalysisNode] Generating embeddings/Response with Ollama...");
            
            // 1. Generate Embedding for Query
            var prompt = state.Input;
            StringBuilder contextBuilder = new StringBuilder();
            
            try 
            {
                if (_embeddingModel != null)
                {
                    /*
                    // Generate embedding (Note: LangChain syntax might vary slightly, checking generic usage)
                    // Assuming CreateEmbeddingsAsync returns float[][] or similar
                    var embeddings = await _embeddingModel.CreateEmbeddingsAsync(prompt);
                    var queryVector = embeddings.First(); // float[]
                    
                    // 2. Vector Search
                    var results = await _cosmosService.SearchAsync(queryVector, topK: 3);
                    
                    if (results.Any())
                    {
                        contextBuilder.AppendLine("Relevant Context/Citations:");
                        foreach (var doc in results)
                        {
                             contextBuilder.AppendLine($"- {doc.Content} (Source: {doc.Metadata.GetValueOrDefault("source", "Unknown")})");
                        }
                    }
                    */
                    Console.WriteLine("[AnalysisNode] (FIX) Vector search temporarily disabled due to shared model compilation error.");
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"[AnalysisNode] Vector Search Failed: {ex.Message}");
                 // Continue without context
            }

            // 3. Generate Analysis with Context
            var fullPrompt = $"Analyze the following input:\n{state.Input}\n\n{contextBuilder}";
            var response = await _model.GenerateAsync(fullPrompt);
            state.Analysis = response.ToString();
        }
        
        return state;
    }
}
