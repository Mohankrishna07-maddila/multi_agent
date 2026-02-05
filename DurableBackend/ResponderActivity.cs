using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableBackend;

public static class ResponderActivity
{
    [Function("ResponderActivity")]
    public static string Run([ActivityTrigger] string context, FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("ResponderActivity");
        logger.LogInformation("ResponderActivity triggered with context: {context}", context);
        return $"ResponderAgent response based on context: {context}";
    }
}
