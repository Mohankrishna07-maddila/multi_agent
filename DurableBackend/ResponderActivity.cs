using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DurableBackend.AI;

namespace DurableBackend;

public class ResponderActivity
{
    private readonly IAiClient _ai;
    private readonly ILogger _logger;

    public ResponderActivity(IAiClient ai, ILoggerFactory loggerFactory)
    {
        _ai = ai;
        _logger = loggerFactory.CreateLogger<ResponderActivity>();
    }

    [Function("ResponderActivity")]
    public async Task<string> Run([ActivityTrigger] string context)
    {
        _logger.LogInformation("ResponderAgent generating response");

        var prompt = $"Generate a helpful response based on this analysis:\n{context}";
        return await _ai.GenerateAsync(prompt);
    }
}
