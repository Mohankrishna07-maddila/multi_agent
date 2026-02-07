using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DurableBackend.AI;

namespace DurableBackend;

public class GraphActivity
{
    private readonly LangGraphAgent _agent;
    private readonly ILogger<GraphActivity> _logger;

    public GraphActivity(LangGraphAgent agent, ILogger<GraphActivity> logger)
    {
        _agent = agent;
        _logger = logger;
    }

    [Function("GraphActivity")]
    public async Task<string> Run([ActivityTrigger] string input)
    {
        try
        {
            _logger.LogInformation("GraphActivity started with input: {Input}", input);
            var result = await _agent.RunAsync(input);
            _logger.LogInformation("GraphActivity completed successfully.");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GraphActivity failed with error: {Message}", ex.Message);
            throw; // Important: Re-throw to let Durable Functions handle the failure
        }
    }
}
