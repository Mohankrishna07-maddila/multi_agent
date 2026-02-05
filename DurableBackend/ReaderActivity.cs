using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableBackend;

public static class ReaderActivity
{
    [Function("ReaderActivity")]
    public static string Run([ActivityTrigger] string input, FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("ReaderActivity");
        logger.LogInformation("ReaderActivity triggered with input: {input}", input);
        return $"ReaderAgent processed: {input}";
    }
}
