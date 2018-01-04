using PasPasPas.Runtime.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Runtime {

    /// <summary>
    ///     test the <c>bits</c> helper
    /// </summary>
    public class BitsTest : CommonTest {

        [TestCase]
        public void BasicTests() {
            var b = new Bits(9);
            Assert.AreEqual(9, b.Length);
            for (var i = 0; i < b.Length; i++)
                Assert.IsFalse(b[i]);
            b.Invert();
            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(b[i]);
            b.Invert();
            for (var i = 0; i < b.Length; i++)
                Assert.IsFalse(b[i]);
            b.Fill();
            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(b[i]);
            b.Clear();
            for (var i = 0; i < b.Length; i++)
                Assert.IsFalse(b[i]);
        }

        /// <summary>
        ///     test access to significant bytes
        /// </summary>
        [TestCase]
        public void LeastSignificantBytesTest() {
            var b = new Bits(99);
            void c() { b.Clear(); for (var k = 0; k < b.Length; k++) Assert.IsFalse(b[k]); };

            c();
            b.LeastSignificantByte = 0x99;
            Assert.AreEqual(0x99, b.LeastSignificantByte);
            c();
            b.LeastSignificantSignedByte = unchecked((sbyte)0x99);
            Assert.AreEqual(unchecked((sbyte)0x99), b.LeastSignificantSignedByte);

            c();
            b.LeastSignificantWord = 0x9999;
            Assert.AreEqual(0x9999, b.LeastSignificantWord);
            c();
            b.LeastSignificantSignedWord = unchecked((short)0x9999);
            Assert.AreEqual(unchecked((short)0x9999), b.LeastSignificantSignedWord);

            c();
            b.LeastSignificantDoubleWord = 0x99999999;
            Assert.AreEqual(0x99999999, b.LeastSignificantDoubleWord);
            c();
            b.LeastSignificantSignedDoubleWord = unchecked((int)0x99999999);
            Assert.AreEqual(unchecked((int)0x99999999), b.LeastSignificantSignedDoubleWord = unchecked((int)0x99999999));

            c();
            b.LeastSignificantQuadWord = 0x9999999999999999;
            Assert.AreEqual(0x9999999999999999, b.LeastSignificantQuadWord);
            c();
            b.LeastSignificantSignedQuadWord = unchecked((long)0x9999999999999999);
            Assert.AreEqual(unchecked((long)0x9999999999999999), b.LeastSignificantSignedQuadWord = unchecked((long)0x9999999999999999));
        }

        [TestCase]
        public void ByteArrayTest() {
            var b = new Bits(15);
            b.Fill();
            Assert.AreEqualSequences(new byte[] { 0xFF, 0x7F }, b.AsByteArray);

            b = new Bits(8);
            b[6] = true;
            Assert.AreEqualSequences(new byte[] { 0x40 }, b.AsByteArray);

            b = new Bits(32);
            b.Fill();
            b[31] = false;
            b[30] = false;
            Assert.AreEqualSequences(new byte[] { 0xFF, 0xFF, 0xFF, 0x3F }, b.AsByteArray);
        }

        [TestCase]
        public void AssignAndEqualsTest() {
            var b1 = new Bits(9);
            var b2 = new Bits(9);
            b1[4] = true;
            b2.Assign(b1);
            Assert.IsFalse(b2[0]);
            Assert.IsFalse(b2[1]);
            Assert.IsFalse(b2[2]);
            Assert.IsFalse(b2[3]);
            Assert.IsTrue(b2[4]);
            Assert.IsFalse(b2[5]);
            Assert.IsFalse(b2[6]);
            Assert.IsFalse(b2[7]);
            Assert.IsFalse(b2[8]);

            Assert.IsTrue(b1.Equals(b2));
            Assert.AreEqual(b1.GetHashCode(), b2.GetHashCode());

            b2.Clear();
            Assert.IsFalse(b1.Equals(b2));
            Assert.AreNotEqual(b1.GetHashCode(), b2.GetHashCode());

        }

    }
}
