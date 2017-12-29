using System;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Runtime.Values;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Runtime {

    public class RuntimeValuesTest : CommonTest {

        [Xunit.Fact]
        public void TestIntegers() {

            // 1 byte signed
            Assert.AreEqual(new byte[] { 127 }, GetIntegerValue((sbyte)127).Data);
            Assert.AreEqual("127", GetIntegerValue((sbyte)127).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((sbyte)127).TypeId);
            Assert.AreEqual(new byte[] { 128 }, GetIntegerValue((sbyte)-128).Data);
            Assert.AreEqual("-128", GetIntegerValue((sbyte)-128).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((sbyte)-128).TypeId);

            // 1 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((byte)0).Data);
            Assert.AreEqual("0", GetIntegerValue((byte)0).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((byte)0).TypeId);
            Assert.AreEqual(new byte[] { 255 }, GetIntegerValue((byte)255).Data);
            Assert.AreEqual("255", GetIntegerValue((byte)255).ToString());
            Assert.AreEqual(TypeIds.ByteType, GetIntegerValue((byte)255).TypeId);

            // 2 byte signed
            Assert.AreEqual(new byte[] { 0, 128 }, GetIntegerValue((short)-32768).Data);
            Assert.AreEqual("-32768", GetIntegerValue((short)-32768).ToString());
            Assert.AreEqual(TypeIds.SmallInt, GetIntegerValue((short)-32768).TypeId);
            Assert.AreEqual(new byte[] { 255, 127 }, GetIntegerValue((short)32767).Data);
            Assert.AreEqual("32767", GetIntegerValue((short)32767).ToString());
            Assert.AreEqual(TypeIds.SmallInt, GetIntegerValue((short)32767).TypeId);

            // 2 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((ushort)0).Data);
            Assert.AreEqual("0", GetIntegerValue((byte)0).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((ushort)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255 }, GetIntegerValue((ushort)65535).Data);
            Assert.AreEqual("65535", GetIntegerValue((ushort)65535).ToString());
            Assert.AreEqual(TypeIds.WordType, GetIntegerValue((ushort)65535).TypeId);

            // 4 byte signed
            Assert.AreEqual(new byte[] { 0, 0, 0, 128 }, GetIntegerValue((int)-2147483648).Data);
            Assert.AreEqual("-2147483648", GetIntegerValue((int)-2147483648).ToString());
            Assert.AreEqual(TypeIds.IntegerType, GetIntegerValue((int)-2147483648).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 127 }, GetIntegerValue((int)2147483647).Data);
            Assert.AreEqual("2147483647", GetIntegerValue((int)2147483647).ToString());
            Assert.AreEqual(TypeIds.IntegerType, GetIntegerValue((int)2147483647).TypeId);

            // 4 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((uint)0).Data);
            Assert.AreEqual("0", GetIntegerValue((uint)0).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((uint)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255 }, GetIntegerValue((uint)4294967295).Data);
            Assert.AreEqual("4294967295", GetIntegerValue((uint)4294967295).ToString());
            Assert.AreEqual(TypeIds.CardinalType, GetIntegerValue((uint)4294967295).TypeId);

            // 8 byte signed
            Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 128 }, GetIntegerValue((long)-9223372036854775808).Data);
            Assert.AreEqual("-9223372036854775808", GetIntegerValue((long)-9223372036854775808).ToString());
            Assert.AreEqual(TypeIds.Int64Type, GetIntegerValue((long)-9223372036854775808).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255, 255, 127 }, GetIntegerValue((long)9223372036854775807).Data);
            Assert.AreEqual("9223372036854775807", GetIntegerValue((long)9223372036854775807).ToString());
            Assert.AreEqual(TypeIds.Int64Type, GetIntegerValue((long)9223372036854775807).TypeId);

            // 8 byte unsigned
            Assert.AreEqual(new byte[] { 0 }, GetIntegerValue((ulong)0).Data);
            Assert.AreEqual("0", GetIntegerValue((ulong)0).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((ulong)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, GetIntegerValue((ulong)18446744073709551615).Data);
            Assert.AreEqual("18446744073709551615", GetIntegerValue((ulong)18446744073709551615).ToString());
            Assert.AreEqual(TypeIds.Uint64Type, GetIntegerValue((ulong)18446744073709551615).TypeId);
        }

        [Xunit.Fact]
        public void TestIntegerNegation() {
            string n(IValue v, int typeKind) {
                var vv = (v as ScaledIntegerValue).Negate();
                Assert.AreEqual(typeKind, vv.TypeId);
                return vv.ToString();
            }

            // 1 byte
            Assert.AreEqual("0", n(GetIntegerValue((sbyte)0), TypeIds.ShortInt));
            Assert.AreEqual("-127", n(GetIntegerValue((sbyte)127), TypeIds.ShortInt));
            Assert.AreEqual("127", n(GetIntegerValue((sbyte)-127), TypeIds.ShortInt));
            Assert.AreEqual("128", n(GetIntegerValue((sbyte)-128), TypeIds.ByteType));
            Assert.AreEqual("-128", n(GetIntegerValue((byte)128), TypeIds.ShortInt));
            Assert.AreEqual("-255", n(GetIntegerValue((byte)255), TypeIds.SmallInt));
            Assert.AreEqual("255", n(GetIntegerValue((short)-255), TypeIds.ByteType));

            // 2 byte
            Assert.AreEqual("0", n(GetIntegerValue((short)0), TypeIds.ShortInt));
            Assert.AreEqual("-32767", n(GetIntegerValue((short)32767), TypeIds.SmallInt));
            Assert.AreEqual("32767", n(GetIntegerValue((short)-32767), TypeIds.SmallInt));
            Assert.AreEqual("32768", n(GetIntegerValue((short)-32768), TypeIds.WordType));
            Assert.AreEqual("-32768", n(GetIntegerValue((ushort)32768), TypeIds.SmallInt));
            Assert.AreEqual("-65535", n(GetIntegerValue((ushort)65535), TypeIds.IntegerType));
            Assert.AreEqual("65535", n(GetIntegerValue((int)-65535), TypeIds.WordType));
            Assert.AreEqual("32769", n(GetIntegerValue((int)-32769), TypeIds.WordType));

            // 4 byte
            Assert.AreEqual("0", n(GetIntegerValue((int)0), TypeIds.ShortInt));
            Assert.AreEqual("-2147483647", n(GetIntegerValue((int)2147483647), TypeIds.IntegerType));
            Assert.AreEqual("2147483647", n(GetIntegerValue((int)-2147483647), TypeIds.IntegerType));
            Assert.AreEqual("2147483648", n(GetIntegerValue((int)-2147483648), TypeIds.CardinalType));
            Assert.AreEqual("-2147483648", n(GetIntegerValue((uint)2147483648), TypeIds.IntegerType));
            Assert.AreEqual("-4294967295", n(GetIntegerValue((uint)4294967295), TypeIds.Int64Type));
            Assert.AreEqual("4294967295", n(GetIntegerValue((long)-4294967295), TypeIds.CardinalType));
            Assert.AreEqual("2147483649", n(GetIntegerValue((long)-2147483649), TypeIds.CardinalType));

            // 8 byte
            Assert.AreEqual("0", n(GetIntegerValue((long)0), TypeIds.ShortInt));
            Assert.AreEqual("-9223372036854775807", n(GetIntegerValue((long)9223372036854775807), TypeIds.Int64Type));
            Assert.AreEqual("9223372036854775807", n(GetIntegerValue((long)-9223372036854775807), TypeIds.Int64Type));
            Assert.AreEqual("9223372036854775808", n(GetIntegerValue((long)-9223372036854775808), TypeIds.Uint64Type));
            Assert.AreEqual("IO", n(GetIntegerValue((ulong)9223372036854775808), TypeIds.ErrorType));
            Assert.AreEqual("IO", n(GetIntegerValue((ulong)18446744073709551615), TypeIds.ErrorType));

        }

    }
}

