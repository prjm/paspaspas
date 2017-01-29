using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using P3Benchmark.Basic;

namespace P3Benchmark {
    class Program {
        static void Main(string[] args) {
            Summary summary = BenchmarkRunner.Run<ListLikeBenchmark>();
        }
    }
}
