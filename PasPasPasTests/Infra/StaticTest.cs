using System;
using PasPasPas.Infrastructure.Environment;
using Xunit;

namespace PasPasPasTests.Infra {

    public class StaticTest {

        private Guid DemoService = new Guid(new byte[] { 0x38, 0xdc, 0xfa, 0x8d, 0x7a, 0x6d, 0x2d, 0x46, 0x8c, 0xd6, 0xd3, 0x31, 0x85, 0xfd, 0xeb, 0x30 });
        /* {8dfadc38-6d7a-462d-8cd6-d33185fdeb30} */

        internal interface IDemo {

        }

        internal class Demo : IDemo {

        }

        [Fact]
        public void TestOptional() {
            Assert.IsNull(StaticEnvironment.Optional<Demo>(DemoService));
            StaticEnvironment.Register(DemoService, () => new Demo());
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
