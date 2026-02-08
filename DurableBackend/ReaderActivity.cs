using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DurableBackend.AI;
using DurableBackend.Memory;
using DurableBackend.Models;

namespace DurableBackend;

public class ReaderActivity
{
    private readonly IAiClient _ai;
    private readonly CosmosMemoryStore _memory;
    private readonly ILogger _logger;

    public ReaderActivity(IAiClient ai, CosmosMemoryStore memory, ILoggerFactory loggerFactory)
    {
        _ai = ai;
        _memory = memory;
        _logger = loggerFactory.CreateLogger<ReaderActivity>();
    }

    [Function("ReaderActivity")]
    public async Task<string> Run([ActivityTrigger] ReaderInput input, FunctionContext context)
    {
        _logger.LogInformation("ReaderAgent analyzing input");

        // Save user message
        var instanceId = input.InstanceId;
        await _memory.SaveAsync(instanceId, "user", input.Content);

        var prompt = $"Analyze the following user input:\n{input.Content}";
        var response = await _ai.GenerateAsync(prompt);

        // Save assistant response
        await _memory.SaveAsync(instanceId, "reader", response);

        return response;
    }
}
