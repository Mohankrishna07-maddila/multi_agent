using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace DurableBackend;

public static class ConversationOrchestrator
{
    [Function("ConversationOrchestrator")]
    public static async Task<object> Run(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var input = context.GetInput<string>();

        var result =
            await context.CallActivityAsync<string>("GraphActivity", input);

        return new { result };
    }
}
