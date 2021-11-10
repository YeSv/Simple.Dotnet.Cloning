using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using Simple.Dotnet.Cloning.Benchmarks;
using System.Linq;

BenchmarkSwitcher.FromAssembly(typeof(AutoMapperCloner).Assembly).Run(args, new AllowNonOptimized());

// For FastDeepCloner which is not optimized
public class AllowNonOptimized : ManualConfig
{
    public AllowNonOptimized()
    {
        Add(JitOptimizationsValidator.DontFailOnError); // ALLOW NON-OPTIMIZED DLLS
        Add(ExecutionValidator.DontFailOnError);

        Add(DefaultConfig.Instance.GetLoggers().ToArray()); // manual config has no loggers by default
        Add(DefaultConfig.Instance.GetExporters().ToArray()); // manual config has no exporters by default
        Add(DefaultConfig.Instance.GetColumnProviders().ToArray()); // manual config has no columns by default
    }
}