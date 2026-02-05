using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableBackend;

public static class ResponderActivity
{
    [FunctionName("ResponderActivity")]
    public static string Run([ActivityTrigger] string context)
    {
        return $"ResponderAgent response based on context: {context}";
    }
}
