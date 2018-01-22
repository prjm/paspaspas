using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPasTests.Common;

namespace PasPasPasTests.Runtime {

    public class RuntimeValuesTest : CommonTest {

        [TestCase]
        public void TestIntegerValues() {

            // 1 byte signed
            Assert.AreEqual(new byte[] { 127 }, GetIntegerValue((sbyte)127).Data);
            Assert.AreEqual("127", GetIntegerValue((sbyte)127).ToString());
            Assert.AreEqual(KnownTypeIds.ShortInt, GetIntegerValue((sbyte)127).TypeId);
            Assert.AreEqual(new byte[] { 128 }, GetIntegerValue((sbyte)-128).Data);
            Assert.AreEqual("-128", GetIntegerValue((sbyte)-128).ToString());
            Assert.AreEqual(KnownTypeIds.ShortInt, GetIntegerValue((sbyte)-128).TypeId);

            // 1 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((byte)0).Data);
            Assert.AreEqual("0", GetIntegerValue((byte)0).ToString());
            Assert.AreEqual(KnownTypeIds.ShortInt, GetIntegerValue((byte)0).TypeId);
            Assert.AreEqual(new byte[] { 255 }, GetIntegerValue((byte)255).Data);
            Assert.AreEqual("255", GetIntegerValue((byte)255).ToString());
            Assert.AreEqual(KnownTypeIds.ByteType, GetIntegerValue((byte)255).TypeId);

            // 2 byte signed
            Assert.AreEqual(new byte[] { 0, 128 }, GetIntegerValue((short)-32768).Data);
            Assert.AreEqual("-32768", GetIntegerValue((short)-32768).ToString());
            Assert.AreEqual(KnownTypeIds.SmallInt, GetIntegerValue((short)-32768).TypeId);
            Assert.AreEqual(new byte[] { 255, 127 }, GetIntegerValue((short)32767).Data);
            Assert.AreEqual("32767", GetIntegerValue((short)32767).ToString());
            Assert.AreEqual(KnownTypeIds.SmallInt, GetIntegerValue((short)32767).TypeId);

            // 2 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((ushort)0).Data);
            Assert.AreEqual("0", GetIntegerValue((byte)0).ToString());
            Assert.AreEqual(KnownTypeIds.ShortInt, GetIntegerValue((ushort)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255 }, GetIntegerValue((ushort)65535).Data);
            Assert.AreEqual("65535", GetIntegerValue((ushort)65535).ToString());
            Assert.AreEqual(KnownTypeIds.WordType, GetIntegerValue((ushort)65535).TypeId);

            // 4 byte signed
            Assert.AreEqual(new byte[] { 0, 0, 0, 128 }, GetIntegerValue((int)-2147483648).Data);
            Assert.AreEqual("-2147483648", GetIntegerValue((int)-2147483648).ToString());
            Assert.AreEqual(KnownTypeIds.IntegerType, GetIntegerValue((int)-2147483648).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 127 }, GetIntegerValue((int)2147483647).Data);
            Assert.AreEqual("2147483647", GetIntegerValue((int)2147483647).ToString());
            Assert.AreEqual(KnownTypeIds.IntegerType, GetIntegerValue((int)2147483647).TypeId);

            // 4 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((uint)0).Data);
            Assert.AreEqual("0", GetIntegerValue((uint)0).ToString());
            Assert.AreEqual(KnownTypeIds.ShortInt, GetIntegerValue((uint)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255 }, GetIntegerValue((uint)4294967295).Data);
            Assert.AreEqual("4294967295", GetIntegerValue((uint)4294967295).ToString());
            Assert.AreEqual(KnownTypeIds.CardinalType, GetIntegerValue((uint)4294967295).TypeId);

            // 8 byte signed
            Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 128 }, GetIntegerValue((long)-9223372036854775808).Data);
            Assert.AreEqual("-9223372036854775808", GetIntegerValue((long)-9223372036854775808).ToString());
            Assert.AreEqual(KnownTypeIds.Int64Type, GetIntegerValue((long)-9223372036854775808).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255, 255, 127 }, GetIntegerValue((long)9223372036854775807).Data);
            Assert.AreEqual("9223372036854775807", GetIntegerValue((long)9223372036854775807).ToString());
            Assert.AreEqual(KnownTypeIds.Int64Type, GetIntegerValue((long)9223372036854775807).TypeId);

            // 8 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((ulong)0).Data);
            Assert.AreEqual("0", GetIntegerValue((ulong)0).ToString());
            Assert.AreEqual(KnownTypeIds.ShortInt, GetIntegerValue((ulong)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, GetIntegerValue((ulong)18446744073709551615).Data);
            Assert.AreEqual("18446744073709551615", GetIntegerValue((ulong)18446744073709551615).ToString());
            Assert.AreEqual(KnownTypeIds.Uint64Type, GetIntegerValue((ulong)18446744073709551615).TypeId);
        }

        [TestCase]
        public void TestIntegerNegation() {
            string n(IValue v, int typeKind) {
                var vv = (v as IIntegerValue).Negate();
                Assert.AreEqual(typeKind, vv.TypeId);
                return vv.ToString();
            }

            // 1 byte
            Assert.AreEqual("0", n(GetIntegerValue((sbyte)0), KnownTypeIds.ShortInt));
            Assert.AreEqual("-127", n(GetIntegerValue((sbyte)127), KnownTypeIds.ShortInt));
            Assert.AreEqual("127", n(GetIntegerValue((sbyte)-127), KnownTypeIds.ShortInt));
            Assert.AreEqual("128", n(GetIntegerValue((sbyte)-128), KnownTypeIds.ByteType));
            Assert.AreEqual("-128", n(GetIntegerValue((byte)128), KnownTypeIds.ShortInt));
            Assert.AreEqual("-255", n(GetIntegerValue((byte)255), KnownTypeIds.SmallInt));
            Assert.AreEqual("255", n(GetIntegerValue((short)-255), KnownTypeIds.ByteType));

            // 2 byte
            Assert.AreEqual("0", n(GetIntegerValue((short)0), KnownTypeIds.ShortInt));
            Assert.AreEqual("-32767", n(GetIntegerValue((short)32767), KnownTypeIds.SmallInt));
            Assert.AreEqual("32767", n(GetIntegerValue((short)-32767), KnownTypeIds.SmallInt));
            Assert.AreEqual("32768", n(GetIntegerValue((short)-32768), KnownTypeIds.WordType));
            Assert.AreEqual("-32768", n(GetIntegerValue((ushort)32768), KnownTypeIds.SmallInt));
            Assert.AreEqual("-65535", n(GetIntegerValue((ushort)65535), KnownTypeIds.IntegerType));
            Assert.AreEqual("65535", n(GetIntegerValue((int)-65535), KnownTypeIds.WordType));
            Assert.AreEqual("32769", n(GetIntegerValue((int)-32769), KnownTypeIds.WordType));

            // 4 byte
            Assert.AreEqual("0", n(GetIntegerValue((int)0), KnownTypeIds.ShortInt));
            Assert.AreEqual("-2147483647", n(GetIntegerValue((int)2147483647), KnownTypeIds.IntegerType));
            Assert.AreEqual("2147483647", n(GetIntegerValue((int)-2147483647), KnownTypeIds.IntegerType));
            Assert.AreEqual("2147483648", n(GetIntegerValue((int)-2147483648), KnownTypeIds.CardinalType));
            Assert.AreEqual("-2147483648", n(GetIntegerValue((uint)2147483648), KnownTypeIds.IntegerType));
            Assert.AreEqual("-4294967295", n(GetIntegerValue((uint)4294967295), KnownTypeIds.Int64Type));
            Assert.AreEqual("4294967295", n(GetIntegerValue((long)-4294967295), KnownTypeIds.CardinalType));
            Assert.AreEqual("2147483649", n(GetIntegerValue((long)-2147483649), KnownTypeIds.CardinalType));

            // 8 byte
            Assert.AreEqual("0", n(GetIntegerValue((long)0), KnownTypeIds.ShortInt));
            Assert.AreEqual("-9223372036854775807", n(GetIntegerValue((long)9223372036854775807), KnownTypeIds.Int64Type));
            Assert.AreEqual("9223372036854775807", n(GetIntegerValue((long)-9223372036854775807), KnownTypeIds.Int64Type));
            Assert.AreEqual("9223372036854775808", n(GetIntegerValue((long)-9223372036854775808), KnownTypeIds.Uint64Type));
            Assert.AreEqual("IO", n(GetIntegerValue((ulong)9223372036854775808), KnownTypeIds.ErrorType));
            Assert.AreEqual("IO", n(GetIntegerValue((ulong)18446744073709551615), KnownTypeIds.ErrorType));

        }

        [TestCase]
        public void TestIntegerAddition() {
            string a(IValue v1, IValue v2) {
                var vv1 = (v1 as INumericalValue).Add(v2);
                var vv2 = (v2 as INumericalValue).Add(v1);
                Assert.AreEqual(vv1.TypeId, vv2.TypeId);
                Assert.AreEqual(vv1.Data, vv2.Data);
                Assert.AreEqual(vv1.ToString(), vv2.ToString());
                return vv1.ToString();
            }

            Assert.AreEqual("25", a(GetIntegerValue(10), GetIntegerValue(15)));
            Assert.AreEqual("-10", a(GetIntegerValue(-3), GetIntegerValue(-7)));
            Assert.AreEqual("-5", a(GetIntegerValue(5), GetIntegerValue(-10)));
            Assert.AreEqual("-8", a(GetIntegerValue(3), GetIntegerValue(-11)));
            Assert.AreEqual("0", a(GetIntegerValue(-3), GetIntegerValue(3)));
            Assert.AreEqual("0", a(GetIntegerValue(3), GetIntegerValue(-3)));

            Assert.AreEqual("IO", a(GetIntegerValue(-9223372036854775808), GetIntegerValue(-4)));
            Assert.AreEqual("IO", a(GetIntegerValue(9223372036854775807), GetIntegerValue(4)));
            Assert.AreEqual("IO", a(GetIntegerValue(9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("IO", a(GetIntegerValue(-9223372036854775808), GetIntegerValue(-9223372036854775808)));
            Assert.AreEqual("0", a(GetIntegerValue(-9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("0", a(GetIntegerValue(9223372036854775807), GetIntegerValue(-9223372036854775807)));
        }

        [TestCase]
        public void TestIntegerSubtraction() {
            string s(IValue v1, IValue v2) {
                var vv1 = (v1 as INumericalValue).Subtract(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("5", s(GetIntegerValue(10), GetIntegerValue(5)));
            Assert.AreEqual("-10", s(GetIntegerValue(-3), GetIntegerValue(7)));
            Assert.AreEqual("5", s(GetIntegerValue(-5), GetIntegerValue(-10)));
            Assert.AreEqual("14", s(GetIntegerValue(3), GetIntegerValue(-11)));
            Assert.AreEqual("0", s(GetIntegerValue(-3), GetIntegerValue(-3)));
            Assert.AreEqual("0", s(GetIntegerValue(3), GetIntegerValue(3)));

            Assert.AreEqual("IO", s(GetIntegerValue(-9223372036854775808), GetIntegerValue(4)));
            Assert.AreEqual("IO", s(GetIntegerValue(9223372036854775807), GetIntegerValue(-4)));
            Assert.AreEqual("IO", s(GetIntegerValue(-9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("IO", s(GetIntegerValue(9223372036854775807), GetIntegerValue(-9223372036854775808)));
            Assert.AreEqual("0", s(GetIntegerValue(9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("0", s(GetIntegerValue(-9223372036854775807), GetIntegerValue(-9223372036854775807)));
        }

        [TestCase]
        public void TestIntegerMultiplication() {
            string m(IValue v1, IValue v2) {
                var vv1 = (v1 as INumericalValue).Multiply(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("50", m(GetIntegerValue(10), GetIntegerValue(5)));
            Assert.AreEqual("0", m(GetIntegerValue(10), GetIntegerValue(0)));
            Assert.AreEqual("0", m(GetIntegerValue(0), GetIntegerValue(10)));
            Assert.AreEqual("0", m(GetIntegerValue(0), GetIntegerValue(0)));
            Assert.AreEqual("-1", m(GetIntegerValue(-1), GetIntegerValue(1)));
            Assert.AreEqual("-1", m(GetIntegerValue(1), GetIntegerValue(-1)));
            Assert.AreEqual("1", m(GetIntegerValue(-1), GetIntegerValue(-1)));
            Assert.AreEqual("IO", m(GetIntegerValue(4867420397139656704), GetIntegerValue(4867420397139656704)));
        }

        [TestCase]
        public void TestIntegerDivision() {
            string d(IValue v1, IValue v2) {
                var vv1 = (v1 as INumericalValue).Divide(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("10", d(GetIntegerValue(30), GetIntegerValue(3)));
            Assert.AreEqual("DZ", d(GetIntegerValue(30), GetIntegerValue(0)));
            Assert.AreEqual("0", d(GetIntegerValue(0), GetIntegerValue(3)));
            Assert.AreEqual("0", d(GetIntegerValue(1), GetIntegerValue(3)));
            Assert.AreEqual("-10", d(GetIntegerValue(30), GetIntegerValue(-3)));
            Assert.AreEqual("-10", d(GetIntegerValue(-30), GetIntegerValue(3)));
            Assert.AreEqual("10", d(GetIntegerValue(-30), GetIntegerValue(-3)));
            Assert.AreEqual("9223372036854775808", d(GetIntegerValue(-9223372036854775808), GetIntegerValue(-1)));
        }

        [TestCase]
        public void TestIntegerModulo() {
            string m(IValue v1, IValue v2) {
                var vv1 = (v1 as INumericalValue).Modulo(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("1", m(GetIntegerValue(5), GetIntegerValue(4)));
            Assert.AreEqual("2", m(GetIntegerValue(32), GetIntegerValue(3)));
            Assert.AreEqual("DZ", m(GetIntegerValue(30), GetIntegerValue(0)));
            Assert.AreEqual("0", m(GetIntegerValue(0), GetIntegerValue(3)));
            Assert.AreEqual("1", m(GetIntegerValue(1), GetIntegerValue(3)));
            Assert.AreEqual("-2", m(GetIntegerValue(32), GetIntegerValue(-3)));
            Assert.AreEqual("-2", m(GetIntegerValue(-32), GetIntegerValue(3)));
            Assert.AreEqual("2", m(GetIntegerValue(-32), GetIntegerValue(-3)));
            Assert.AreEqual("1", m(GetIntegerValue(-9223372036854775808), GetIntegerValue(9223372036854775809)));
        }

        [TestCase]
        public void TestIntegerNot() {
            string m(IValue v, int typeKind) {
                var vv1 = (v as IIntegerValue).Not();
                Assert.AreEqual(typeKind, vv1.TypeId);
                return vv1.ToString();
            }

            Assert.AreEqual("-128", m(GetIntegerValue(127), KnownTypeIds.ShortInt));
            Assert.AreEqual("-129", m(GetIntegerValue(128), KnownTypeIds.SmallInt));
            Assert.AreEqual("128", m(GetIntegerValue(-129), KnownTypeIds.ByteType));
            Assert.AreEqual("-9223372036854775808", m(GetIntegerValue(9223372036854775807), KnownTypeIds.Int64Type));
            Assert.AreEqual("9223372036854775807", m(GetIntegerValue(9223372036854775808), KnownTypeIds.Int64Type));
            Assert.AreEqual("0", m(GetIntegerValue(18446744073709551615), KnownTypeIds.ShortInt));
        }

        [TestCase]
        public void TestIntegerAnd() {
            string a(IValue v1, IValue v2) {
                var vv1 = (v1 as IIntegerValue).And(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("2", a(GetIntegerValue(127), GetIntegerValue(2)));
            Assert.AreEqual("127", a(GetIntegerValue(127), GetIntegerValue(127)));
            Assert.AreEqual("0", a(GetIntegerValue(0), GetIntegerValue(18446744073709551615)));
            Assert.AreEqual("0", a(GetIntegerValue(18446744073709551615), GetIntegerValue(0)));
            Assert.AreEqual("-1", a(GetIntegerValue(18446744073709551615), GetIntegerValue(18446744073709551615)));
        }

        [TestCase]
        public void TestIntegerOr() {
            string o(IValue v1, IValue v2) {
                var vv1 = (v1 as IIntegerValue).Or(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("125", o(GetIntegerValue(124), GetIntegerValue(5)));
            Assert.AreEqual("0", o(GetIntegerValue(0), GetIntegerValue(0)));
            Assert.AreEqual("127", o(GetIntegerValue(127), GetIntegerValue(0)));
            Assert.AreEqual("255", o(GetIntegerValue(240), GetIntegerValue(15)));
        }

        [TestCase]
        public void TestIntegerXor() {
            string x(IValue v1, IValue v2) {
                var vv1 = (v1 as IIntegerValue).Xor(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("0", x(GetIntegerValue(240), GetIntegerValue(240)));
            Assert.AreEqual("255", x(GetIntegerValue(240), GetIntegerValue(15)));
            Assert.AreEqual("0", x(GetIntegerValue(0), GetIntegerValue(0)));
            Assert.AreEqual("-2", x(GetIntegerValue(-127), GetIntegerValue(127)));
        }

        [TestCase]
        public void TestIntegerShl() {
            string sl(IValue v1, IValue v2) {
                var vv1 = (v1 as IIntegerValue).Shl(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("-2", sl(GetIntegerValue(2147483647), GetIntegerValue(1)));
            Assert.AreEqual("8", sl(GetIntegerValue(4), GetIntegerValue(-255)));
            Assert.AreEqual("8", sl(GetIntegerValue(4), GetIntegerValue(1)));
        }

        [TestCase]
        public void TestIntegerShr() {
            string sr(IValue v1, IValue v2) {
                var vv1 = (v1 as IIntegerValue).Shr(v2);
                return vv1.ToString();
            }

            Assert.AreEqual("1", sr(GetIntegerValue(2), GetIntegerValue(1)));
            Assert.AreEqual("2147483647", sr(GetIntegerValue(-1), GetIntegerValue(1)));
        }

    }

}

