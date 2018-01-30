using BenchmarkDotNet.Running;
using P3Benchmark.Runtime;

namespace P3Benchmark {
    class Program {
        static void Main(string[] args) {
            //new ScaledIntegers().TestAddsNew();
            var summary = BenchmarkRunner.Run<ScaledIntegers>();
        }
    }
}
