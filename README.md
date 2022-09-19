# benchmarking console app example
A .net core console app that uses BenchmarkDotNet to benchmark code performance, etc: https://benchmarkdotnet.org/

## Description
To run, open Powershell at the Benchy.Console project directory and run:

```powershell
dotnet run -c release
```

Benchmarking results will be output to the terminal window and also to a new created \Benchy.Console\BenchmarkDotNet.Artifacts directory
