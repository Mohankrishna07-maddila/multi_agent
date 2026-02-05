namespace DurableBackend.AI;

public class MockAiClient : IAiClient
{
    public Task<string> GenerateAsync(string prompt)
    {
        // Simulates an AI response
        return Task.FromResult($"[AI OUTPUT] {prompt}");
    }
}
