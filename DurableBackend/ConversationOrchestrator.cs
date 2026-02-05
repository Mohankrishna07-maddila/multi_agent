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

        var readerResult =
            await context.CallActivityAsync<string>("ReaderActivity", input);

        var responderResult =
            await context.CallActivityAsync<string>("ResponderActivity", readerResult);

        return new
        {
            input,
            result = responderResult
        };
    }
}
