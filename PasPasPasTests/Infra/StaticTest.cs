using System;
using PasPasPas.Infrastructure.Environment;
using Xunit;

namespace PasPasPasTests.Infra {

    public class StaticTest {

        private int DemoService = 999;

        internal interface IDemo {

        }

        internal class Demo : IDemo {

        }

        [Fact]
        public void TestOptional() {
            var env = new StaticEnvironment();
            Assert.IsNull(env.Optional<Demo>(DemoService));
            env.Register(DemoService, new Demo());
            Assert.AreEqual(typeof(Demo), env.Optional<Demo>(DemoService)?.GetType());
            Assert.AreEqual(typeof(Demo), env.Optional<IDemo>(DemoService)?.GetType());
            Assert.AreEqual(null, env.Optional<string>(DemoService)?.GetType());
            env.Clear();
            Assert.IsNull(env.Optional<Demo>(DemoService));
        }

        [Fact]
        public void TestRequired() {
            var env = new StaticEnvironment();
            env.Clear();
            Assert.Throws<InvalidOperationException>(() => env.Require<Demo>(DemoService));
        }

    }
}
