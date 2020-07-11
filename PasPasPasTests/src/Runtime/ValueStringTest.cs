using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Runtime {

    /// <summary>
    ///     test value strings
    /// </summary>
    public class ValueStringTest : CommonTest {

        private void AssertValueString(string expected, string typeName, IValue value) {
            Assert.AreEqual(typeName, (value.TypeDefinition as INamedTypeSymbol)?.Name ?? string.Empty, StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(expected, value.ToValueString());
        }


        /// <summary>
        ///     test integral values
        /// </summary>
        [TestMethod]
        public void TestIntegralValues() {
            AssertValueString("129", "Byte", GetIntegerValue(129));
            AssertValueString("2147483649", "Cardinal", GetIntegerValue(2147483649));
            AssertValueString("4294967297", "Int64", GetIntegerValue(4294967297));
            AssertValueString("2147483647", "Integer", GetIntegerValue(2147483647));
            AssertValueString("3", "ShortInt", GetIntegerValue(3));
            AssertValueString("534", "SmallInt", GetIntegerValue(534));
            AssertValueString("9223372036854775808", "UInt64", GetIntegerValue(9223372036854775808));
            AssertValueString("65535", "Word", GetIntegerValue(65535));
        }

        /// <summary>
        ///     test boolean values
        /// </summary>
        [TestMethod]
        public void TestBooleanValues() {
            AssertValueString("True", "Boolean", GetBooleanValue(true));
            AssertValueString("False", "Boolean", GetBooleanValue(false));
            AssertValueString("3", "ByteBool", GetByteBooleanValue(3));
            AssertValueString("3", "WordBool", GetWordBooleanValue(3));
            AssertValueString("3", "LongBool", GetLongBooleanValue(3));
        }

        /// <summary>
        ///     test char values
        /// </summary>
        [TestMethod]
        public void TestCharValues() {
            AssertValueString("c", "AnsiChar", GetAnsiCharValue((byte)'c'));
            AssertValueString("q", "WideChar", GetWideCharValue('q'));
        }

        /// <summary>
        ///     test float values
        /// </summary>
        [TestMethod]
        public void TestFloatValues() {
            AssertValueString("1.30000000000000004441", "Extended", GetExtendedValue(1.3));
            AssertValueString("-1.30000000000000004441", "Extended", GetExtendedValue(-1.3));

        }

        /// <summary>
        ///     test enumerated values
        /// </summary>
        [TestMethod]
        public void TestEnumeratedValues() {
            var values = GetEnumValues("a", "b", "c", "");
            AssertValueString("b", string.Empty, values[1]);
            AssertValueString("3", string.Empty, values[3]);
        }

        /// <summary>
        ///     test miscellaneous values
        /// </summary>
        [TestMethod]
        public void TestMiscValues() {
            AssertValueString("Nil", string.Empty, GetNilValue());
            AssertValueString("***", "Error", GetErrorValue());
            AssertValueString("@99", "Pointer", GetPointerValue(GetIntegerValue(99)));
            AssertValueString("2", string.Empty, GetSubrangeValue(GetIntegerValue(-3), GetIntegerValue(3), GetIntegerValue(2)));
        }

        /// <summary>
        ///     test string values
        /// </summary>
        [TestMethod]
        public void TestStringValues() {
            AssertValueString("aa", "Ansistring", GetAnsiStringValue("aa"));
            AssertValueString("", "ShortString", GetEmptyStringValue());
            AssertValueString("qq", "ShortString", GetShortStringValue("qq"));
            AssertValueString("1234", "unicodestring", GetUnicodeStringValue("1234"));
        }

        /// <summary>
        ///     test structured values
        /// </summary>
        [TestMethod]
        public void TestStructuredValues() {
            AssertValueString("[1,3,1]", "", GetArrayValue(GetIntegerValue(1), GetIntegerValue(3), GetIntegerValue(1)));
            AssertValueString("(a:1; b:x; c:33)", "", GetRecordValue(("a", GetIntegerValue(1)), ("b", GetUnicodeStringValue("x")), ("c", GetIntegerValue(33))));
            AssertValueString("[a,b,c]", "", GetSetValue("a", "b", "c"));
        }

    }
}
