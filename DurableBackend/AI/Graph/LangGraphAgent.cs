using DurableBackend.AI.Graph;

namespace DurableBackend.AI;

public class LangGraphAgent
{
    private readonly AnalysisNode _analysisNode;
    private readonly ResponseNode _responseNode;

    public LangGraphAgent(AnalysisNode analysisNode, ResponseNode responseNode)
    {
        _analysisNode = analysisNode;
        _responseNode = responseNode;
    }

    public async Task<string> RunAsync(string input)
    {
        var graph = new StateGraph<AgentState>();

        // Use injected nodes
        graph.AddNode("analysis", _analysisNode.Run);
        graph.AddNode("response", _responseNode.Run);

        graph.SetEntryPoint("analysis");
        graph.AddEdge("analysis", "response");
        graph.SetFinishPoint("response");

        var executor = graph.Compile();

        var result = await executor.InvokeAsync(new AgentState
        {
            Input = input
        });

        return result.Response;
    }
}
