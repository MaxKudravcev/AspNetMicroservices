using Ocelot.DependencyInjection;
using Ocelot.Middleware;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        builder.Services.AddOcelot();
        
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        await app.UseOcelot();
        
        app.Run();
    }
}