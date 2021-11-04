using BenchmarkDotNet.Running;
using Simple.Dotnet.Cloning.Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(AutoMapperCloner).Assembly).Run(args);