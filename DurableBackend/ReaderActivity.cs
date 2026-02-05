using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DurableBackend.AI;

namespace DurableBackend;

public class ReaderActivity
{
    private readonly IAiClient _ai;
    private readonly ILogger _logger;

    public ReaderActivity(IAiClient ai, ILoggerFactory loggerFactory)
    {
        _ai = ai;
        _logger = loggerFactory.CreateLogger<ReaderActivity>();
    }

    [Function("ReaderActivity")]
    public async Task<string> Run([ActivityTrigger] string input)
    {
        _logger.LogInformation("ReaderAgent analyzing input");

        var prompt = $"Analyze the following user input:\n{input}";
        return await _ai.GenerateAsync(prompt);
    }
}
