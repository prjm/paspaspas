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
            Assert.IsNull(StaticEnvironment.Optional<Demo>(DemoService));
            StaticEnvironment.Register(DemoService, new Demo());
            Assert.AreEqual(typeof(Demo), StaticEnvironment.Optional<Demo>(DemoService)?.GetType());
            Assert.AreEqual(typeof(Demo), StaticEnvironment.Optional<IDemo>(DemoService)?.GetType());
            Assert.AreEqual(null, StaticEnvironment.Optional<string>(DemoService)?.GetType());
            StaticEnvironment.Clear();
            Assert.IsNull(StaticEnvironment.Optional<Demo>(DemoService));
        }

        [Fact]
        public void TestRequired() {
            StaticEnvironment.Clear();
            Assert.Throws<InvalidOperationException>(() => StaticEnvironment.Require<Demo>(DemoService));
        }

    }
}
