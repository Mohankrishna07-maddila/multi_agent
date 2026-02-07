
using DurableBackend.AI.Graph;

namespace DurableBackend.AI;

public class LangGraphAgent
{
    public async Task<string> RunAsync(string input)
    {
        var graph = new StateGraph<AgentState>();

        graph.AddNode("analysis", new AnalysisNode().Run);
        graph.AddNode("response", new ResponseNode().Run);

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
