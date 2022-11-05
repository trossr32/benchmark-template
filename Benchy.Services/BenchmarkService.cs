using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;

namespace Benchy.Services;

public interface IBenchmarkService
{
    Task Process();
}

public class BenchmarkService : IBenchmarkService
{
    private readonly ILogger<BenchmarkService> _logger;

    public BenchmarkService(ILogger<BenchmarkService> logger)
    {
        _logger = logger;
    }

    public async Task Process()
    {
        var summary = BenchmarkRunner.Run<StringBench>();
    }
}

/// <summary>
/// Benchmarks StringBuilder vs String.Create using Spans to deconstruct a string and put back together with a space every 4th character.
/// </summary>
[MemoryDiagnoser]
public class StringBench
{
    private const string UnformattedKey = "ifuahsdfiuahsdifuhsaidufhasiudfhasiudhfiasuhdfisuahdfiasuhdfx";

    [Benchmark]
    public void StringBuilder_Test()
    {
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < UnformattedKey.Length)
        {
            result.Append(UnformattedKey.Substring(currentPosition, 4)).Append(" ");
            currentPosition += 4;
        }
        if (currentPosition < UnformattedKey.Length)
        {
            result.Append(UnformattedKey.Substring(currentPosition));
        }

        var solution = result.ToString().ToLowerInvariant();
    }

    [Benchmark]
    public void StringCreate_Test()
    {
        var length = (int)Math.Floor((decimal)UnformattedKey.Length / 4) + UnformattedKey.Length;

        var solution = string.Create(length, UnformattedKey, (chars, state) =>
        {
            var position = 0;
            var marker = 0;

            while (position < length)
            {
                var toCopy = state.Length - marker > 4 ? 4 : state.Length - marker;

                state.AsSpan().Slice(marker, toCopy).CopyTo(chars[position..]);

                position += toCopy;
                marker += toCopy;

                if (position < length)
                    chars[position++] = ' ';
            }
        }).ToLowerInvariant();
    }
}