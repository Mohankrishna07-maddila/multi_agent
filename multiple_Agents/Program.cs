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

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // ✅ ADD THIS BLOCK
        app.MapGet("/api/orchestrate/{input}", (
            string input,
            multiple_Agents.Agents.ReaderAgent reader,
            multiple_Agents.Agents.ResponderAgent responder) =>
        {
            var context = reader.Read(input);
            var result = responder.Respond(context);
            return Results.Ok(result);
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
