using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using PasPasPas.Infrastructure.Utils;

namespace P3Benchmark.Basic {
    public class ListLikeBenchmark {

        private long TestListOperations(IList<object> list, int size) {
            for (var i = 0; i < size; i++) {
                list.Add(new object());
            }

            var sample = 0L;
            foreach (var item in list) {
                sample *= item.GetHashCode();
            }
            foreach (var item in list) {
                sample *= item.GetHashCode();
            }

            sample *= list[0].GetHashCode();
            sample *= list[list.Count - 1].GetHashCode();

            list.Clear();

            sample += list.Count;
            return sample;
        }

        [Params(1, 2, 10, 25)]
        public int TestCount { get; set; }

        [Benchmark]
        public long TestStandardList()
            => TestListOperations(new List<object>(), TestCount);

        [Benchmark]
        public long TestSmallList()
            => TestListOperations(new SmallListCollection<object>(), TestCount);


    }
}
