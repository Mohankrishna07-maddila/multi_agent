using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using DurableBackend.Models;

namespace DurableBackend;

public static class ConversationOrchestrator
{
    [Function("ConversationOrchestrator")]
    public static async Task<object> Run(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var input = context.GetInput<string>();

        var readerInput = new ReaderInput
        {
            InstanceId = context.InstanceId,
            Content = input
        };

        // Call ReaderActivity instead of GraphActivity for now to test Cosmos integration
        var result = await context.CallActivityAsync<string>("ReaderActivity", readerInput);

        return new { result };
    }
}
