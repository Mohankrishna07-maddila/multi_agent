using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DurableBackend.AI;

namespace DurableBackend;

public class GraphActivity
{
    private readonly LangGraphAgent _agent;

    public GraphActivity(LangGraphAgent agent)
    {
        _agent = agent;
    }

    [Function("GraphActivity")]
    public async Task<string> Run([ActivityTrigger] string input)
    {
        return await _agent.RunAsync(input);
    }
}
