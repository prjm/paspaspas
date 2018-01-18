using System;
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
            Assert.IsFalse(b.IsFilled);
            Assert.IsTrue(b.IsCleared);

            b.Invert();
            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(b[i]);
            Assert.IsFalse(b.IsCleared);
            Assert.IsTrue(b.IsFilled);

            b.Invert();
            for (var i = 0; i < b.Length; i++)
                Assert.IsFalse(b[i]);
            Assert.IsFalse(b.IsFilled);
            Assert.IsTrue(b.IsCleared);

            b.Fill();
            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(b[i]);
            Assert.IsFalse(b.IsCleared);
            Assert.IsTrue(b.IsFilled);

            b.Clear();
            for (var i = 0; i < b.Length; i++)
                Assert.IsFalse(b[i]);
            Assert.IsFalse(b.IsFilled);
            Assert.IsTrue(b.IsCleared);

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

        [TestCase]
        public void DivideTest() {
            var b1 = new Bits(8);
            var b2 = new Bits(8);

            b1.LeastSignificantByte = 10;
            b2.LeastSignificantByte = 2;
            var b3 = b1.Divide(b2);
            Assert.AreEqual(5, b3.LeastSignificantByte);

            b1 = new Bits(8);
            b2 = new Bits(8);

            b1.LeastSignificantSignedByte = 22;
            b2.LeastSignificantSignedByte = 11;
            b3 = b1.Divide(b2);
            Assert.AreEqual(2, b3.LeastSignificantByte);
        }

        [TestCase]
        public void ModTest() {
            var b1 = new Bits(8);
            var b2 = new Bits(8);
            b1.LeastSignificantByte = 5;
            b2.LeastSignificantByte = 3;
            var b3 = b1.Modulo(b2);
            Assert.AreEqual(2, b3.LeastSignificantByte);

            b1 = new Bits(8);
            b2 = new Bits(8);
            b1.LeastSignificantByte = 5;
            b2.LeastSignificantByte = 5;
            b3 = b1.Modulo(b2);
            Assert.AreEqual(0, b3.LeastSignificantByte);

            b1 = new Bits(8);
            b2 = new Bits(8);
            b1.LeastSignificantSignedByte = -10;
            b2.LeastSignificantByte = 1;
            b3 = b1.Modulo(b2);
            Assert.AreEqual(0, b3.LeastSignificantByte);

            b1 = new Bits(8);
            b2 = new Bits(8);
            b1.LeastSignificantSignedByte = -10;
            b2.LeastSignificantByte = 3;
            b3 = b1.Modulo(b2);
            Assert.AreEqual(-1, b3.LeastSignificantSignedByte);
        }

        [TestCase]
        public void AddTest() {
            var b1 = new Bits(8);
            var b2 = new Bits(8);

            b1.LeastSignificantByte = 23;
            b2.LeastSignificantByte = 44;
            b1.Add(b2);
            Assert.AreEqual(b1.LeastSignificantByte, 67);

            b1 = new Bits(15);
            b2 = new Bits(15);
            b1[14] = true;
            b2[14] = true;
            b1.Add(b2);
            Assert.AreEqual(new byte[] { 0, 0 }, b1.AsByteArray);


            b1 = new Bits(72);
            b2 = new Bits(72);
            b1.Invert();
            b2[0] = true;
            b1.Add(b2);
            Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, b1.AsByteArray);
        }

        [TestCase]
        public void FillTest() {
            var b2 = new Bits(3);
            var b1 = new Bits(9);

            b2.LeastSignificantByte = 0x5;

            b1.FillFromLeft(b2);
            Assert.AreEqual(320, b1.LeastSignificantWord);
            b1.Clear();

            b1.FillFromRight(b2);
            Assert.AreEqual(5, b1.LeastSignificantWord);
            b1.Clear();
        }

        [TestCase]
        public void TwoComplementTest() {
            var b = new Bits(9);
            b.TwoComplement();
            Assert.AreEqual(0, b.LeastSignificantWord);

            b = new Bits(8) {
                LeastSignificantSignedByte = 1
            };
            b.TwoComplement();
            Assert.AreEqual(-1, b.LeastSignificantSignedByte);

            b = new Bits(8) {
                LeastSignificantSignedByte = -128
            };
            b.TwoComplement();
            Assert.AreEqual(-128, b.LeastSignificantSignedByte);

        }

        [TestCase]
        public void TestNumbers() {
            var b = new Bits(33) {
                MostSignificantBit = true
            };
            Assert.IsTrue(b.IsMostNegative);
            b.TwoComplement();
            Assert.IsTrue(b.IsMostNegative);

            b = new Bits(945) {
                MostSignificantBit = true
            };
            Assert.IsTrue(b.IsMostNegative);
            b.TwoComplement();
            Assert.IsTrue(b.IsMostNegative);

            b = new Bits(33);
            b.Invert();
            b.MostSignificantBit = false;
            Assert.IsFalse(b.IsMostNegative);
            Assert.IsTrue(b.IsMostPositive);
            b.TwoComplement();
            Assert.IsFalse(b.IsMostNegative);

            b = new Bits(945);
            b.Invert();
            b.MostSignificantBit = false;
            Assert.IsFalse(b.IsMostNegative);
            Assert.IsTrue(b.IsMostPositive);
            b.TwoComplement();
            Assert.IsFalse(b.IsMostNegative);

        }

        [TestCase]
        public void TestMultiply() {
            var b1 = new Bits(4);
            var b2 = new Bits(4);
            Bits b3;

            b1.LeastSignificantSignedByte = -8;
            b2.LeastSignificantSignedByte = 2;
            b3 = b1.Multiply(b2);
            Assert.AreEqual(-16, b3.LeastSignificantSignedByte);

            b1.LeastSignificantSignedByte = 3;
            b2.LeastSignificantSignedByte = -4;
            b3 = b1.Multiply(b2);
            Assert.AreEqual(-12, b3.LeastSignificantSignedByte);
        }

        private void RunBinaryOpForByte(Func<sbyte, sbyte, int> desired, Func<Bits, Bits, Bits> toBeChecked) {
            var b1 = new Bits(32);
            var b2 = new Bits(32);

            for (sbyte i = -128; i < 127; i++) {
                for (sbyte j = -128; j < 127; j++) {
                    b1.Clear();
                    b2.Clear();

                    var result = desired(i, j);

                    if (i < 0)
                        b1.Invert();
                    b1.LeastSignificantSignedByte = i;

                    if (j < 0)
                        b2.Invert();
                    b2.LeastSignificantSignedByte = j;
                    var b3 = toBeChecked(b1, b2);

                    Assert.AreEqual(result, b3.LeastSignificantSignedQuadWord);
                }
            }


        }

        /*
        [TestCase(Skip = "Takes some time")]
        public void TestMultiplyByte() {
            int desired(sbyte b1, sbyte b2) => b1 * b2;
            Bits chkd(Bits b1, Bits b2) => b1.Multiply(b2);
            RunBinaryOpForByte(desired, chkd);
        }
        */

        [TestCase]
        public void TestResize() {
            var b = new Bits(9) {
                LeastSignificantByte = 0b0_1010_1010
            };
            b.Length = 4;
            Assert.AreEqual(0b_1010, b.LeastSignificantByte);
            b.Length = 9;
            Assert.AreEqual(0b_0_0000_1010, b.LeastSignificantByte);
        }

    }
}
