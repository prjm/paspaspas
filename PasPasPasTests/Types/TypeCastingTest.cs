using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values;
using PasPasPas.Runtime.Values.BooleanValues;
using PasPasPas.Runtime.Values.CharValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     tests for type casting
    /// </summary>
    public class TypeCastingTest : TypeTest {

        [TestCase]
        public void TestIntegerCastingDirect() {
            AssertExprValue("ShortInt(384)", new ShortIntValue(-128), "", KnownTypeIds.ShortInt);
            AssertExprValue("Byte(384)", new ByteValue(128), "", KnownTypeIds.ByteType);
            AssertExprValue("SmallInt(384)", new SmallIntValue(384), "", KnownTypeIds.SmallInt);
            AssertExprValue("Word(384)", new WordValue(384), "", KnownTypeIds.WordType);
            AssertExprValue("Integer(384)", new IntegerValue(384), "", KnownTypeIds.IntegerType);
            AssertExprValue("Cardinal(384)", new CardinalValue(384), "", KnownTypeIds.CardinalType);
            AssertExprValue("Int64(384)", new Int64Value(384), "", KnownTypeIds.Int64Type);
            AssertExprValue("UInt64(384)", new UInt64Value(384), "", KnownTypeIds.Uint64Type);

            AssertExprValue("WideChar(384)", new WideCharValue((char)384), "", KnownTypeIds.WideCharType);
            AssertExprValue("Char(384)", new WideCharValue((char)384), "", KnownTypeIds.WideCharType);
            AssertExprValue("AnsiChar(384)", new AnsiCharValue(unchecked((byte)384)), "", KnownTypeIds.AnsiCharType);

            AssertExprValue("Boolean(384)", new BooleanValue(true), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool(384)", new ByteBooleanValue(unchecked((byte)384)), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool(384)", new WordBooleanValue(unchecked((ushort)384)), "", KnownTypeIds.WordBoolType);

            AssertExprValue("e(384)",
                new EnumeratedValue(RegisteredTypes.SmallestUserTypeId, new ShortIntValue(unchecked((sbyte)384))),
                "type e = (e1, e2);", RegisteredTypes.SmallestUserTypeId);
        }

        [TestCase]
        public void TestIntegerCastingIndirect() {
            AssertExprTypeByVar("Integer", "ShortInt(a)", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("Integer", "Byte(a)", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Integer", "SmallInt(a)", KnownTypeIds.SmallInt);
            AssertExprTypeByVar("Integer", "Word(a)", KnownTypeIds.WordType);
            AssertExprTypeByVar("Byte", "Integer(a)", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Byte", "Cardinal(a)", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("Byte", "Int64(a)", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "UInt64(a)", KnownTypeIds.Uint64Type);

            AssertExprTypeByVar("Byte", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("Byte", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("Byte", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("Integer", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Integer", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("Integer", "WordBool(a)", KnownTypeIds.WordBoolType);

            AssertExprTypeByVar("Integer", "e(a)", RegisteredTypes.SmallestUserTypeId, false, "type e = (e1, e2);");
        }

    }
}
