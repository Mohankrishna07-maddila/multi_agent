using Microsoft.Azure.Cosmos;
using DurableBackend.Models;

namespace DurableBackend.Services;

public class CosmosDbService
{
    private readonly Container _historyContainer;
    private readonly Container _vectorContainer;

    public CosmosDbService(CosmosClient dbClient, string databaseName)
    {
        var database = dbClient.GetDatabase(databaseName);
        _historyContainer = database.GetContainer("ConversationHistory");
        _vectorContainer = database.GetContainer("VectorStore");
    }

    // Initialize Database and Containers (Call this from Program.cs or Startup)
    public static async Task InitializeAsync(CosmosClient client, string databaseName)
    {
        DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        
        // History Container
        await database.Database.CreateContainerIfNotExistsAsync("ConversationHistory", "/sessionId");

        // Vector Container with Vector Indexing Policy
        ContainerProperties vectorContainerProperties = new ContainerProperties("VectorStore", "/sessionId");
        
        // Define Vector Embedding Policy
        VectorEmbeddingPolicy vectorEmbeddingPolicy = new VectorEmbeddingPolicy(new System.Collections.ObjectModel.Collection<Embedding>
        {
            new Embedding
            {
                Path = "/vector",
                DataType = VectorDataType.Float32,
                Dimensions = 384, // all-minilm dimension
                DistanceFunction = DistanceFunction.Cosine
            }
        });
        vectorContainerProperties.VectorEmbeddingPolicy = vectorEmbeddingPolicy;

        // Define Indexing Policy
        vectorContainerProperties.IndexingPolicy.VectorIndexes.Add(new VectorIndexPath
        {
            Path = "/vector",
            Type = VectorIndexType.QuantizedFlat // or Flat
        });

        await database.Database.CreateContainerIfNotExistsAsync(vectorContainerProperties);
    }

    public async Task AddItemAsync<T>(T item)
    {
        // Determine container based on type
        if (item is ConversationRecord record)
        {
            await _historyContainer.CreateItemAsync(record, new PartitionKey(record.SessionId));
        }
        else if (item is VectorDocument vectorDoc)
        {
            await _vectorContainer.CreateItemAsync(vectorDoc, new PartitionKey(vectorDoc.SessionId)); // Using SessionId partition for now, or use a fixed one for global knowledge
        }
    }

    public async Task<List<VectorDocument>> SearchAsync(float[] queryVector, int topK = 3)
    {
        // Vector Search Query
        // Note: ORDER BY VectorDistance is supported in newer SDKs/Service versions
        var queryText = $"SELECT TOP {topK} c.id, c.content, c.metadata, c.sessionId, VectorDistance(c.vector, @queryVector) AS SimilarityScore FROM c ORDER BY VectorDistance(c.vector, @queryVector)";
        
        var queryDef = new QueryDefinition(queryText)
            .WithParameter("@queryVector", queryVector);

        var iterator = _vectorContainer.GetItemQueryIterator<VectorDocument>(queryDef);
        
        List<VectorDocument> results = new List<VectorDocument>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }
}
