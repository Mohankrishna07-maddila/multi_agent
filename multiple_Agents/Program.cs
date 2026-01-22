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


        var app = builder.Build();

        app.MapDefaultEndpoints();

        // ✅ ADD THIS BLOCK
        app.MapGet("/api/orchestrate/{input}", (
            string input,
            multiple_Agents.Agents.ReaderAgent reader,
            multiple_Agents.Agents.ResponderAgent responder,
            multiple_Agents.Memory.ConversationMemory memory) =>
        {
            memory.Add(input);

            var context = reader.Read(input);
            var result = responder.Respond(context);

            var history = string.Join(" | ", memory.GetAll());

            return Results.Ok(new
            {
                result,
                memory = history
            });
        });


        // Configure the HTTP request pipeline.
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
