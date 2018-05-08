using PasPasPas.Infrastructure.Utils;
using Xunit;

namespace PasPasPasTests.Infra {
    public class ScopedNames {

        [Fact]
        public void TestScopedSimpleNames() {
            var name1 = new ScopedName("teSt");
            var name2 = new ScopedName("test");
            var name3 = new ScopedName("test1");
            Assert.Equal(name1, name2);
            Assert.NotEqual(name1, name3);
            Assert.NotEqual(name2, name3);
            Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
            Assert.NotEqual(name1.GetHashCode(), name3.GetHashCode());
            Assert.NotEqual(name2.GetHashCode(), name3.GetHashCode());
            Assert.Equal("teSt", name1.ToString());
            Assert.Equal("test", name2.ToString());
            Assert.Equal("test1", name3.ToString());
        }

        [Fact]
        public void TestPrefixedNames() {
            var name1 = new ScopedName("a", "teSt");
            var name2 = new ScopedName("A", "test");
            var name3 = new ScopedName("B", "test1");
            Assert.Equal(name1, name2);
            Assert.NotEqual(name1, name3);
            Assert.NotEqual(name2, name3);
            Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
            Assert.NotEqual(name1.GetHashCode(), name3.GetHashCode());
            Assert.NotEqual(name2.GetHashCode(), name3.GetHashCode());
            Assert.Equal("a.teSt", name1.ToString());
            Assert.Equal("A.test", name2.ToString());
            Assert.Equal("B.test1", name3.ToString());
        }
    }
}
