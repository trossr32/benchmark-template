using Benchy.Console.Options;
using Benchy.Core.Models.Configuration;
using Benchy.Core.Services.Configuration;
using Benchy.Services;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Benchy.Console;

class Program
{
    private static readonly IServiceProvider ServiceProvider;
    private static readonly IConfigurationRoot Configuration;
    private static ILogger<Program> _logger;

    static Program()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
            .AddJsonFile("file.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        var fileSettings = Configuration.GetSection(nameof(FileSettings)).Get<FileSettings>();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(fileSettings.LogDirectory, $"{DateTime.Now:yyyyMMdd_HHmmss}.log"))   // log to file system
            .WriteTo.Console()                                                                              // log to console
            .CreateLogger();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();

        // logging
        services.AddLogging(configure => configure.AddSerilog());

        // configuration
        services.Configure<FileSettings>(options => Configuration.GetSection(nameof(FileSettings)).Bind(options));
        services.AddSingleton<IFileSettingsService, FileSettingsService>();

        // services
        services.AddSingleton<IBenchmarkService, BenchmarkService>();
    }

    /// <summary>
    /// App entry point. Use CommandLineParser to configure any command line arguments.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    static async Task Main(string[] args)
    {
        _logger = ServiceProvider.GetService<ILogger<Program>>();

        await Parser.Default.ParseArguments<BenchmarkOptions>(args)
            .MapResult(
                async options => await Benchmark(options),
                err => Task.FromResult(-1)
            );
    }

    /// <summary>
    /// Run on 'run' verb, e.g. App.Console.exe run
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private static async Task Benchmark(BenchmarkOptions options)
    {
        var processSvc = ServiceProvider.GetService<IBenchmarkService>();

        switch (options.Action)
        {
            case RunAction.AnAction:
                try
                {
                    await processSvc.Process();
                }
                catch (Exception e)
                {
                    _logger?.LogError(e, "Failed to run process");

                    throw;
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}