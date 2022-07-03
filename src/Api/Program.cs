using Repository.Database;
using Repository.Seed;
using Serilog;

namespace Api;
public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        Log.Information("Starting Api");

        var host = CreateHostBuilder(args).Build();
        host.SeedDatabase();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    private static void SeedDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var userContext = services.GetRequiredService<UserContext>();
        userContext.Seed();
    }
}