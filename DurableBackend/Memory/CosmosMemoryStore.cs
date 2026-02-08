using Microsoft.Azure.Cosmos;

namespace DurableBackend.Memory;

public class CosmosMemoryStore
{
    private readonly Container _container;

    public CosmosMemoryStore(string connectionString)
    {
        var client = new CosmosClient(connectionString);
        _container = client
            .GetDatabase("multiAgentDB")
            .GetContainer("conversations");
    }

    public async Task SaveAsync(
        string conversationId,
        string role,
        string content)
    {
        var item = new
        {
            id = Guid.NewGuid().ToString(),
            conversationId,
            role,
            content,
            timestamp = DateTime.UtcNow
        };

        await _container.CreateItemAsync(
            item,
            new PartitionKey(conversationId));
    }

    public async Task<List<string>> ReadConversationAsync(string conversationId)
    {
        var query = new QueryDefinition(
            "SELECT c.content FROM c WHERE c.conversationId = @id ORDER BY c.timestamp")
            .WithParameter("@id", conversationId);

        var results = new List<string>();
        var iterator = _container.GetItemQueryIterator<dynamic>(query);

        while (iterator.HasMoreResults)
        {
            foreach (var item in await iterator.ReadNextAsync())
            {
                results.Add(item.content.ToString());
            }
        }

        return results;
    }
}
