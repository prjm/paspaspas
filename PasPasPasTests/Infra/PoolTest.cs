using System.Text;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.ObjectPooling;
using Xunit;
using Assert = PasPasPasTests.Common.Assert;

namespace PasPasPasTests.Infra {

    /// <summary>
    ///     test object pooling
    /// </summary>
    public class PoolTest {

        [Fact]
        public void TestStringBuilderBool() {
            var pool = new StringBuilderPool();
            var teststring = "test123test";
            object reference = null;

            using (var sb = pool.Borrow()) {
                reference = sb.Item;
                sb.Item.Append(teststring);
                Assert.AreEqual(sb.Item.ToString(), teststring);
            }

            using (var sb = pool.Borrow()) {
                Assert.IsTrue(ReferenceEquals(sb.Item, reference));
                sb.Item.Append(teststring);
                Assert.AreEqual(sb.Item.ToString(), teststring);
            }

        }
    }
}
