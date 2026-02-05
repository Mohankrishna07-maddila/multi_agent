using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableBackend;

public static class ReaderActivity
{
    [FunctionName("ReaderActivity")]
    public static string Run([ActivityTrigger] string input)
    {
        return $"ReaderAgent processed: {input}";
    }
}
