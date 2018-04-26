using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using PasPasPas.Runtime.Values;
using PasPasPas.Runtime.Values.Int;

namespace P3Benchmark.Runtime {

    [MemoryDiagnoser]
    public class ScaledIntegers {


        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max) {
            lock (syncLock) { // synchronize
                return getrandom.Next(min, max);
            }
        }

        public void TestAddition2() {
            var a = GetRandomNumber(int.MinValue + 100, int.MaxValue - 100);
            var b = GetRandomNumber(-99, 99);
            var c = a + b;
            var d1 = new IntegerValue(a);
            var d2 = new IntegerValue(b);
            var r = d1.AsBigInteger + d2.AsBigInteger;
            var d3 = IntegerValueBase.ToIntValue(r);
            var e = d3 as IntegerValueBase;
            var f = e.SignedValue;
            //if (c != f)
            //   Console.WriteLine(a.ToString() + " + " + b.ToString() + " = " + c.ToString() + " != " + f.ToString());
        }

        [Benchmark]
        public void TestAddsNew() {
            for (var i = 0; i < 150; i++)
                TestAddition2();
        }

    }
}
