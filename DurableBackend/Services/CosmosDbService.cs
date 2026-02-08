using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace DurableBackend.Services;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(
        CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync<T>(T item)
    {
        await _container.CreateItemAsync(item, new PartitionKey(((dynamic)item).SessionId));
    }
}
