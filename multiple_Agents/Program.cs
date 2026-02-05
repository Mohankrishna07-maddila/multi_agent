using multiple_Agents.Components;

namespace multiple_Agents;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddSingleton<multiple_Agents.Agents.ReaderAgent>();
        builder.Services.AddSingleton<multiple_Agents.Agents.ResponderAgent>();
        builder.Services.AddSingleton<multiple_Agents.Memory.ConversationMemory>();
        builder.Services.AddSingleton<multiple_Agents.Orchestration.ConversationOrchestrator>();
        builder.Services.AddHttpClient("durable", client =>
        {
            client.BaseAddress = new Uri("http://localhost:7136");
        });


        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Backend API
        app.MapGet("/api/hello", () =>
        {
            return Results.Ok("Hello from multiple-agents backend");
        });

        app.MapGet("/api/orchestrate/{sessionId}/{input}", (
            string sessionId, 
            string input, 
            multiple_Agents.Orchestration.ConversationOrchestrator orchestrator) =>
        {
            return Results.Ok(orchestrator.Run(sessionId, input));
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
