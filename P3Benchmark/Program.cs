using BenchmarkDotNet.Running;
using P3Benchmark.Runtime;

namespace P3Benchmark {
    class Program {
        static void Main() {
            //new ScaledIntegers().TestAddsNew();
            var summary = BenchmarkRunner.Run<ScaledIntegers>();
        }
    }
}
