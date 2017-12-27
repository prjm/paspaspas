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
            Assert.AreEqual(TypeIds.ByteType, GetIntegerValue((byte)0).TypeId);
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
            Assert.AreEqual(new byte[] { 0, 0 }, GetIntegerValue((ushort)0).Data);
            Assert.AreEqual("0", GetIntegerValue((byte)0).ToString());
            Assert.AreEqual(TypeIds.WordType, GetIntegerValue((ushort)0).TypeId);
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
            Assert.AreEqual(new byte[] { 0, 0, 0, 0 }, GetIntegerValue((uint)0).Data);
            Assert.AreEqual("0", GetIntegerValue((uint)0).ToString());
            Assert.AreEqual(TypeIds.CardinalType, GetIntegerValue((uint)0).TypeId);
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
            Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, GetIntegerValue((ulong)0).Data);
            Assert.AreEqual("0", GetIntegerValue((ulong)0).ToString());
            Assert.AreEqual(TypeIds.Uint64Type, GetIntegerValue((ulong)0).TypeId);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, GetIntegerValue((ulong)18446744073709551615).Data);
            Assert.AreEqual("18446744073709551615", GetIntegerValue((ulong)18446744073709551615).ToString());
            Assert.AreEqual(TypeIds.Uint64Type, GetIntegerValue((ulong)18446744073709551615).TypeId);
        }

    }
}

