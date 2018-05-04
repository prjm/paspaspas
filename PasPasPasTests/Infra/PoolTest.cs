using System.Text;
using PasPasPas.Infrastructure.Environment;
using Xunit;
using Assert = PasPasPasTests.Common.Assert;

namespace PasPasPasTests.Infra {

    /// <summary>
    ///     test object pooling
    /// </summary>
    public class PoolTest {

        [Fact]
        public void TestStringBuilderBool() {
            var pool = new ObjectPool<StringBuilder>();
            var teststring = "test123test";
            object reference = null;

            using (var sb = pool.Borrow()) {
                reference = sb.Data;
                sb.Data.Append(teststring);
                Assert.AreEqual(sb.Data.ToString(), teststring);
            }

            using (var sb = pool.Borrow()) {
                Assert.IsTrue(object.ReferenceEquals(sb.Data, reference));
                sb.Data.Append(teststring);
                Assert.AreEqual(sb.Data.ToString(), teststring);
            }

        }
    }
}
