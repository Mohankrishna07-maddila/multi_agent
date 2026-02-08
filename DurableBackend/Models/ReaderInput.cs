namespace DurableBackend.Models;

public class ReaderInput
{
    public string InstanceId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
