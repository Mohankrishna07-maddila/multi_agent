using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DurableBackend;

public static class HttpStart
{
    [FunctionName("StartConversation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "start/{input}")]
        HttpRequest req,
        string input,
        [DurableClient] IDurableOrchestrationClient starter)
    {
        string instanceId =
            await starter.StartNewAsync("ConversationOrchestrator", input);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
