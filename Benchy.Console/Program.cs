using Benchy.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddOptions();
        services.AddSingleton<BenchmarkService>();
    })
    .ConfigureLogging((_, logging) =>
    {
        logging.AddSerilog(dispose: true);
    })
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var b = host.Services.GetService<BenchmarkService>();
b!.Process();

await host.RunAsync();