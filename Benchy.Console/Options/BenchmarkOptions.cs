using CommandLine;

namespace Benchy.Console.Options;

[Verb("benchmark", HelpText = "Benchmark functions.")]
public class BenchmarkOptions
{
    [Option('a', "action", Required = false, HelpText = "Identify the action to perform.")]
    public RunAction Action { get; set; }
}

public enum RunAction
{
    AnAction = 0
}