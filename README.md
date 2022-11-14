# benchmarking console app example
A .net 6 core console app that uses BenchmarkDotNet to benchmark code performance, etc: https://benchmarkdotnet.org/

## Description
To run, open Powershell at the Benchy.Console project directory and run:

```powershell
dotnet run -c release
```

Benchmarking results will be output to the terminal window and also to a new created \Benchy.Console\BenchmarkDotNet.Artifacts directory

## Which framework?

The main branch is up to date with .net 6, use branch **feature/net7.0** to benchmark .net 7 code
