using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();


builder.Services.AddSingleton<DurableBackend.AI.IAiClient, DurableBackend.AI.LangChainAiClient>();
builder.Services.AddSingleton<DurableBackend.AI.LangGraphAgent>();

// Cosmos DB Registration
builder.Services.AddSingleton(sp => 
{
    // Use Emulator default if not set (for local dev convenience)
    var connectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
    if (string.IsNullOrEmpty(connectionString))
    {
        // Emulator default
        connectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    }
    return new Microsoft.Azure.Cosmos.CosmosClient(connectionString);
});

builder.Services.AddSingleton<DurableBackend.Services.CosmosDbService>(sp =>
{
    var client = sp.GetRequiredService<Microsoft.Azure.Cosmos.CosmosClient>();
    // We create the service instance. It expects database and containers to exist or be created.
    // We'll handle creation below.
    return new DurableBackend.Services.CosmosDbService(client, "AgentDatabase");
});

var host = builder.Build();

// Initialize Cosmos DB (Create Database and Containers)
try 
{
    var cosmosClient = host.Services.GetRequiredService<Microsoft.Azure.Cosmos.CosmosClient>();
    await DurableBackend.Services.CosmosDbService.InitializeAsync(cosmosClient, "AgentDatabase");
}
catch (Exception ex)
{
    Console.WriteLine($"[Program] Cosmos DB Initialization Failed: {ex.Message}");
    // Continue running, maybe without DB
}

host.Run();
