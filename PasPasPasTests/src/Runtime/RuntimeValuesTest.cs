using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Runtime {

    /// <summary>
    ///     runtime values test
    /// </summary>
    public class RuntimeValuesTest : CommonTest {

        private ISystemUnit KnownTypeIds
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test integer values
        /// </summary>
        [TestMethod]
        public void TestIntegerValues() {
            static string v(IValue s)
                => s.ToValueString();

            // 1 byte signed
            Assert.AreEqual("127", v(GetIntegerValue((sbyte)127)));
            Assert.AreEqual(KnownTypeIds.ShortIntType, GetIntegerValue((sbyte)127).TypeDefinition);
            Assert.AreEqual("-128", v(GetIntegerValue((sbyte)-128)));
            Assert.AreEqual(KnownTypeIds.ShortIntType, GetIntegerValue((sbyte)-128).TypeDefinition);

            // 1 byte unsigned
            Assert.AreEqual("0", v(GetIntegerValue((byte)0)));
            Assert.AreEqual(KnownTypeIds.ShortIntType, GetIntegerValue((byte)0).TypeDefinition);
            Assert.AreEqual("255", v(GetIntegerValue((byte)255)));
            Assert.AreEqual(KnownTypeIds.ByteType, GetIntegerValue((byte)255).TypeDefinition);

            // 2 byte signed
            Assert.AreEqual("-32768", v(GetIntegerValue((short)-32768)));
            Assert.AreEqual(KnownTypeIds.SmallIntType, GetIntegerValue((short)-32768).TypeDefinition);
            Assert.AreEqual("32767", v(GetIntegerValue((short)32767)));
            Assert.AreEqual(KnownTypeIds.SmallIntType, GetIntegerValue((short)32767).TypeDefinition);

            // 2 byte unsigned
            Assert.AreEqual("0", v(GetIntegerValue((byte)0)));
            Assert.AreEqual(KnownTypeIds.ShortIntType, GetIntegerValue((ushort)0).TypeDefinition);
            Assert.AreEqual("65535", v(GetIntegerValue((ushort)65535)));
            Assert.AreEqual(KnownTypeIds.WordType, GetIntegerValue((ushort)65535).TypeDefinition);

            // 4 byte signed
            Assert.AreEqual("-2147483648", v(GetIntegerValue(-2147483648)));
            Assert.AreEqual(KnownTypeIds.IntegerType, GetIntegerValue(-2147483648).TypeDefinition);
            Assert.AreEqual("2147483647", v(GetIntegerValue(2147483647)));
            Assert.AreEqual(KnownTypeIds.IntegerType, GetIntegerValue(2147483647).TypeDefinition);

            // 4 byte unsigned
            Assert.AreEqual("0", v(GetIntegerValue((uint)0)));
            Assert.AreEqual(KnownTypeIds.ShortIntType, GetIntegerValue((uint)0).TypeDefinition);
            Assert.AreEqual("4294967295", v(GetIntegerValue(4294967295)));
            Assert.AreEqual(KnownTypeIds.CardinalType, GetIntegerValue(4294967295).TypeDefinition);

            // 8 byte signed
            Assert.AreEqual("-9223372036854775808", v(GetIntegerValue(-9223372036854775808)));
            Assert.AreEqual(KnownTypeIds.Int64Type, GetIntegerValue(-9223372036854775808).TypeDefinition);
            Assert.AreEqual("9223372036854775807", v(GetIntegerValue(9223372036854775807)));
            Assert.AreEqual(KnownTypeIds.Int64Type, GetIntegerValue(9223372036854775807).TypeDefinition);

            // 8 byte unsigned
            Assert.AreEqual("0", v(GetIntegerValue((ulong)0)));
            Assert.AreEqual(KnownTypeIds.ShortIntType, GetIntegerValue((ulong)0).TypeDefinition);
            Assert.AreEqual("18446744073709551615", v(GetIntegerValue(18446744073709551615)));
            Assert.AreEqual(KnownTypeIds.UInt64Type, GetIntegerValue(18446744073709551615).TypeDefinition);
        }

        /// <summary>
        ///     test integer negation
        /// </summary>
        [TestMethod]
        public void TestIntegerNegation() {
            static string n(IValue v, ITypeDefinition typeKind) {
                var c = CreateEnvironment().TypeRegistry.Runtime.Integers;
                var vv = c.Negate(v);
                Assert.AreEqual(typeKind, vv.TypeDefinition);
                return vv.ToValueString();
            }

            // 1 byte
            Assert.AreEqual("0", n(GetIntegerValue((sbyte)0), KnownTypeIds.ShortIntType));
            Assert.AreEqual("-127", n(GetIntegerValue((sbyte)127), KnownTypeIds.ShortIntType));
            Assert.AreEqual("127", n(GetIntegerValue((sbyte)-127), KnownTypeIds.ShortIntType));
            Assert.AreEqual("128", n(GetIntegerValue((sbyte)-128), KnownTypeIds.ByteType));
            Assert.AreEqual("-128", n(GetIntegerValue((byte)128), KnownTypeIds.ShortIntType));
            Assert.AreEqual("-255", n(GetIntegerValue((byte)255), KnownTypeIds.SmallIntType));
            Assert.AreEqual("255", n(GetIntegerValue((short)-255), KnownTypeIds.ByteType));

            // 2 byte
            Assert.AreEqual("0", n(GetIntegerValue((short)0), KnownTypeIds.ShortIntType));
            Assert.AreEqual("-32767", n(GetIntegerValue((short)32767), KnownTypeIds.SmallIntType));
            Assert.AreEqual("32767", n(GetIntegerValue((short)-32767), KnownTypeIds.SmallIntType));
            Assert.AreEqual("32768", n(GetIntegerValue((short)-32768), KnownTypeIds.WordType));
            Assert.AreEqual("-32768", n(GetIntegerValue((ushort)32768), KnownTypeIds.SmallIntType));
            Assert.AreEqual("-65535", n(GetIntegerValue((ushort)65535), KnownTypeIds.IntegerType));
            Assert.AreEqual("65535", n(GetIntegerValue(-65535), KnownTypeIds.WordType));
            Assert.AreEqual("32769", n(GetIntegerValue(-32769), KnownTypeIds.WordType));

            // 4 byte
            Assert.AreEqual("0", n(GetIntegerValue(0), KnownTypeIds.ShortIntType));
            Assert.AreEqual("-2147483647", n(GetIntegerValue(2147483647), KnownTypeIds.IntegerType));
            Assert.AreEqual("2147483647", n(GetIntegerValue(-2147483647), KnownTypeIds.IntegerType));
            Assert.AreEqual("2147483648", n(GetIntegerValue(-2147483648), KnownTypeIds.CardinalType));
            Assert.AreEqual("-2147483648", n(GetIntegerValue(2147483648), KnownTypeIds.IntegerType));
            Assert.AreEqual("-4294967295", n(GetIntegerValue(4294967295), KnownTypeIds.Int64Type));
            Assert.AreEqual("4294967295", n(GetIntegerValue(-4294967295), KnownTypeIds.CardinalType));
            Assert.AreEqual("2147483649", n(GetIntegerValue(-2147483649), KnownTypeIds.CardinalType));

            // 8 byte
            Assert.AreEqual("0", n(GetIntegerValue((long)0), KnownTypeIds.ShortIntType));
            Assert.AreEqual("-9223372036854775807", n(GetIntegerValue(9223372036854775807), KnownTypeIds.Int64Type));
            Assert.AreEqual("9223372036854775807", n(GetIntegerValue(-9223372036854775807), KnownTypeIds.Int64Type));
            Assert.AreEqual("9223372036854775808", n(GetIntegerValue(-9223372036854775808), KnownTypeIds.UInt64Type));
            Assert.AreEqual("***", n(GetIntegerValue(9223372036854775809), KnownTypeIds.ErrorType));
            Assert.AreEqual("***", n(GetIntegerValue(18446744073709551615), KnownTypeIds.ErrorType));

        }

        /// <summary>
        ///     test integer addition
        /// </summary>
        [TestMethod]
        public void TestIntegerAddition() {
            static string a(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Add(v1, v2);
                var vv2 = c.Add(v2, v1);
                Assert.AreEqual(vv1.TypeDefinition, vv2.TypeDefinition);
                Assert.AreEqual(vv1.ToString(), vv2.ToString());
                return vv1.ToValueString();
            }

            Assert.AreEqual("25", a(GetIntegerValue(10), GetIntegerValue(15)));
            Assert.AreEqual("-10", a(GetIntegerValue(-3), GetIntegerValue(-7)));
            Assert.AreEqual("-5", a(GetIntegerValue(5), GetIntegerValue(-10)));
            Assert.AreEqual("-8", a(GetIntegerValue(3), GetIntegerValue(-11)));
            Assert.AreEqual("0", a(GetIntegerValue(-3), GetIntegerValue(3)));
            Assert.AreEqual("0", a(GetIntegerValue(3), GetIntegerValue(-3)));

            Assert.AreEqual("***", a(GetIntegerValue(-9223372036854775808), GetIntegerValue(-4)));
            Assert.AreEqual("***", a(GetIntegerValue(18446744073709551615), GetIntegerValue(4)));
            Assert.AreEqual("***", a(GetIntegerValue(9223372036854775808), GetIntegerValue(9223372036854775808)));
            Assert.AreEqual("***", a(GetIntegerValue(-9223372036854775808), GetIntegerValue(-9223372036854775808)));
            Assert.AreEqual("0", a(GetIntegerValue(-9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("0", a(GetIntegerValue(9223372036854775807), GetIntegerValue(-9223372036854775807)));
        }

        /// <summary>
        ///     test integer subtract
        /// </summary>
        [TestMethod]
        public void TestIntegerSubtraction() {
            static string s(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Subtract(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("5", s(GetIntegerValue(10), GetIntegerValue(5)));
            Assert.AreEqual("-10", s(GetIntegerValue(-3), GetIntegerValue(7)));
            Assert.AreEqual("5", s(GetIntegerValue(-5), GetIntegerValue(-10)));
            Assert.AreEqual("14", s(GetIntegerValue(3), GetIntegerValue(-11)));
            Assert.AreEqual("0", s(GetIntegerValue(-3), GetIntegerValue(-3)));
            Assert.AreEqual("0", s(GetIntegerValue(3), GetIntegerValue(3)));

            Assert.AreEqual("***", s(GetIntegerValue(-9223372036854775808), GetIntegerValue(4)));
            Assert.AreEqual("***", s(GetIntegerValue(18446744073709551615), GetIntegerValue(-4)));
            Assert.AreEqual("***", s(GetIntegerValue(-9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("-8", s(GetIntegerValue(9223372036854775800), GetIntegerValue(9223372036854775808)));
            Assert.AreEqual("0", s(GetIntegerValue(9223372036854775807), GetIntegerValue(9223372036854775807)));
            Assert.AreEqual("0", s(GetIntegerValue(-9223372036854775807), GetIntegerValue(-9223372036854775807)));
        }

        /// <summary>
        ///     test integer multiplication
        /// </summary>
        [TestMethod]
        public void TestIntegerMultiplication() {
            static string m(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Multiply(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("50", m(GetIntegerValue(10), GetIntegerValue(5)));
            Assert.AreEqual("0", m(GetIntegerValue(10), GetIntegerValue(0)));
            Assert.AreEqual("0", m(GetIntegerValue(0), GetIntegerValue(10)));
            Assert.AreEqual("0", m(GetIntegerValue(0), GetIntegerValue(0)));
            Assert.AreEqual("-1", m(GetIntegerValue(-1), GetIntegerValue(1)));
            Assert.AreEqual("-1", m(GetIntegerValue(1), GetIntegerValue(-1)));
            Assert.AreEqual("1", m(GetIntegerValue(-1), GetIntegerValue(-1)));
            Assert.AreEqual("***", m(GetIntegerValue(4867420397139656704), GetIntegerValue(4867420397139656704)));
        }

        /// <summary>
        ///     test integer division
        /// </summary>
        [TestMethod]
        public void TestIntegerDivision() {
            static string d(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Divide(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("10", d(GetIntegerValue(30), GetIntegerValue(3)));
            Assert.AreEqual("***", d(GetIntegerValue(30), GetIntegerValue(0)));
            Assert.AreEqual("0", d(GetIntegerValue(0), GetIntegerValue(3)));
            Assert.AreEqual("0", d(GetIntegerValue(1), GetIntegerValue(3)));
            Assert.AreEqual("-10", d(GetIntegerValue(30), GetIntegerValue(-3)));
            Assert.AreEqual("-10", d(GetIntegerValue(-30), GetIntegerValue(3)));
            Assert.AreEqual("10", d(GetIntegerValue(-30), GetIntegerValue(-3)));
            Assert.AreEqual("9223372036854775808", d(GetIntegerValue(-9223372036854775808), GetIntegerValue(-1)));
        }

        /// <summary>
        ///     test integer modulo
        /// </summary>
        [TestMethod]
        public void TestIntegerModulo() {
            static string m(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Modulo(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("1", m(GetIntegerValue(5), GetIntegerValue(4)));
            Assert.AreEqual("2", m(GetIntegerValue(32), GetIntegerValue(3)));
            Assert.AreEqual("***", m(GetIntegerValue(30), GetIntegerValue(0)));
            Assert.AreEqual("0", m(GetIntegerValue(0), GetIntegerValue(3)));
            Assert.AreEqual("1", m(GetIntegerValue(1), GetIntegerValue(3)));
            Assert.AreEqual("2", m(GetIntegerValue(32), GetIntegerValue(-3)));
            Assert.AreEqual("-2", m(GetIntegerValue(-32), GetIntegerValue(3)));
            Assert.AreEqual("-2", m(GetIntegerValue(-32), GetIntegerValue(-3)));
            Assert.AreEqual("-9223372036854775808", m(GetIntegerValue(-9223372036854775808), GetIntegerValue(9223372036854775809)));
        }

        /// <summary>
        ///     test the <c>not</c> operator
        /// </summary>
        [TestMethod]
        public void TestIntegerNot() {
            static string m(IValue v, ITypeDefinition typeKind) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.NotOperator(v);
                Assert.AreEqual(typeKind, vv1.TypeDefinition);
                return vv1.ToValueString();
            }

            Assert.AreEqual("-128", m(GetIntegerValue(127), KnownTypeIds.ShortIntType));
            Assert.AreEqual("-129", m(GetIntegerValue(128), KnownTypeIds.SmallIntType));
            Assert.AreEqual("128", m(GetIntegerValue(-129), KnownTypeIds.ByteType));
            Assert.AreEqual("-9223372036854775808", m(GetIntegerValue(9223372036854775807), KnownTypeIds.Int64Type));
            Assert.AreEqual("9223372036854775807", m(GetIntegerValue(9223372036854775808), KnownTypeIds.Int64Type));
            Assert.AreEqual("1", m(GetIntegerValue(18446744073709551614), KnownTypeIds.ShortIntType));
        }

        /// <summary>
        ///     test integer and
        /// </summary>
        [TestMethod]
        public void TestIntegerAnd() {
            static string a(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.AndOperator(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("2", a(GetIntegerValue(127), GetIntegerValue(2)));
            Assert.AreEqual("127", a(GetIntegerValue(127), GetIntegerValue(127)));
            Assert.AreEqual("0", a(GetIntegerValue(0), GetIntegerValue(18446744073709551615)));
            Assert.AreEqual("0", a(GetIntegerValue(18446744073709551615), GetIntegerValue(0)));
            Assert.AreEqual("18446744073709551615", a(GetIntegerValue(18446744073709551615), GetIntegerValue(18446744073709551615)));
        }

        /// <summary>
        ///     test integer or
        /// </summary>
        [TestMethod]
        public void TestIntegerOr() {
            static string o(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.OrOperator(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("125", o(GetIntegerValue(124), GetIntegerValue(5)));
            Assert.AreEqual("0", o(GetIntegerValue(0), GetIntegerValue(0)));
            Assert.AreEqual("127", o(GetIntegerValue(127), GetIntegerValue(0)));
            Assert.AreEqual("255", o(GetIntegerValue(240), GetIntegerValue(15)));
        }

        /// <summary>
        ///     test <c>xor</c> operator
        /// </summary>
        [TestMethod]
        public void TestIntegerXor() {
            static string x(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.XorOperator(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("0", x(GetIntegerValue(240), GetIntegerValue(240)));
            Assert.AreEqual("255", x(GetIntegerValue(240), GetIntegerValue(15)));
            Assert.AreEqual("0", x(GetIntegerValue(0), GetIntegerValue(0)));
            Assert.AreEqual("-2", x(GetIntegerValue(-127), GetIntegerValue(127)));
        }

        /// <summary>
        ///     test the <c>shl</c> operator
        /// </summary>
        [TestMethod]
        public void TestIntegerShl() {
            static string sl(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Shl(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("-2", sl(GetIntegerValue(2147483647), GetIntegerValue(1)));
            Assert.AreEqual("8", sl(GetIntegerValue(4), GetIntegerValue(-255)));
            Assert.AreEqual("8", sl(GetIntegerValue(4), GetIntegerValue(1)));
        }

        /// <summary>
        ///     test integer shift right
        /// </summary>
        [TestMethod]
        public void TestIntegerShr() {
            static string sr(IValue v1, IValue v2) {
                var c = CreateEnvironment().Runtime.Integers;
                var vv1 = c.Shr(v1, v2);
                return vv1.ToValueString();
            }

            Assert.AreEqual("1", sr(GetIntegerValue(2), GetIntegerValue(1)));
            Assert.AreEqual("2147483647", sr(GetIntegerValue(-1), GetIntegerValue(1)));
            Assert.AreEqual("1", sr(GetIntegerValue(1), GetIntegerValue(32)));
        }

    }

}

