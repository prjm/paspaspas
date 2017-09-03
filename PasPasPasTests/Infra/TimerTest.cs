using System.Diagnostics;
using System.Threading;
using PasPasPas.Infrastructure.Environment;
using Xunit;

namespace PasPasPasTests.Infra {
    public class TimerTest {

        [Fact]
        public void TestSimple() {
            var timer = new ExecutionTimer();
            var count = 200;
            var stopwatch = new Stopwatch();

            Assert.IsTrue(timer.HitCount == 0);
            Assert.IsTrue(timer.TickCount == 0);

            stopwatch.Start();
            for (var i = 0; i < count; i++) {
                timer.Start();
                Thread.Sleep(1);
                timer.Stop();
            }
            stopwatch.Stop();

            Assert.IsTrue(timer.HitCount == 200);
            Assert.IsTrue(timer.TickCount > 0);
            Assert.IsTrue(timer.TickCount <= stopwatch.ElapsedTicks);
        }

        [Fact]
        public void TestRecursive() {
            var root = new ExecutionTimer();
            var call = new ExecutionTimer(root);

            int fib(int n) {
                call.Start();
                try {
                    Assert.IsTrue(root.IsRunning);
                    Assert.IsTrue(call.IsRunning);

                    if (n == 1 || n == 2)
                        return n;
                    else
                        return fib(n - 1) + fib(n - 2);
                }
                finally {
                    call.Stop();
                }
            }

            fib(5);

            Assert.IsFalse(root.IsRunning);
            Assert.IsFalse(call.IsRunning);
            Assert.IsTrue(root.TickCount >= call.TickCount);
            Assert.AreEqual(9L, call.HitCount);
            Assert.AreEqual(0, root.HitCount);
        }
    }
}
