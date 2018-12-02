using PasPasPas.Infrastructure.Utils;
using PasPasPasTests.Common;

namespace PasPasPasTests.Infra {
    public class ScopedNames {

        [TestMethod]
        public void TestScopedSimpleNames() {
            var name1 = new ScopedName("teSt");
            var name2 = new ScopedName("test");
            var name3 = new ScopedName("test1");
            Assert.AreEqual(name1, name2);
            Assert.AreNotEqual(name1, name3);
            Assert.AreNotEqual(name2, name3);
            Assert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
            Assert.AreNotEqual(name1.GetHashCode(), name3.GetHashCode());
            Assert.AreNotEqual(name2.GetHashCode(), name3.GetHashCode());
            Assert.AreEqual("teSt", name1.ToString());
            Assert.AreEqual("test", name2.ToString());
            Assert.AreEqual("test1", name3.ToString());
        }

        [TestMethod]
        public void TestPrefixedNames() {
            var name1 = new ScopedName("a", "teSt");
            var name2 = new ScopedName("A", "test");
            var name3 = new ScopedName("B", "test1");
            Assert.AreEqual(name1, name2);
            Assert.AreNotEqual(name1, name3);
            Assert.AreNotEqual(name2, name3);
            Assert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
            Assert.AreNotEqual(name1.GetHashCode(), name3.GetHashCode());
            Assert.AreNotEqual(name2.GetHashCode(), name3.GetHashCode());
            Assert.AreEqual("a.teSt", name1.ToString());
            Assert.AreEqual("A.test", name2.ToString());
            Assert.AreEqual("B.test1", name3.ToString());
        }
    }
}
