using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using System.Net;

namespace DurableBackend;

public static class HttpStart
{
    [Function("StartConversation")]
    public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "start/{input}")]
        HttpRequestData req,
        string input,
        [DurableClient] DurableTaskClient client)
    {
        string instanceId =
            await client.ScheduleNewOrchestrationInstanceAsync(
                "ConversationOrchestrator",
                input);

        // Return management URLs with authentication codes
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}
