namespace DurableBackend.AI;

public interface IAiClient
{
    Task<string> GenerateAsync(string prompt);
}
