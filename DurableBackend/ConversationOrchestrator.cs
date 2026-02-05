using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableBackend;

public static class ConversationOrchestrator
{
    [FunctionName("ConversationOrchestrator")]
    public static async Task<object> Run(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
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
