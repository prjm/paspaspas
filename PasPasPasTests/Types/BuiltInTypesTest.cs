using PasPasPas.Typings.Common;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for built in types
    /// </summary>
    public class BuiltInTypesTest : TypeTest {

        [Fact]
        public void TestIntTypes() {
            AssertDeclType("System.Byte", TypeIds.ByteType);
            AssertDeclType("System.Word", TypeIds.WordType);
            AssertDeclType("System.Cardinal", TypeIds.CardinalType);
            AssertDeclType("System.UInt64", TypeIds.Uint64Type);
            AssertDeclType("System.ShortInt", TypeIds.ShortInt);
            AssertDeclType("System.SmallInt", TypeIds.SmallInt);
            AssertDeclType("System.Integer", TypeIds.IntegerType);
            AssertDeclType("System.Int64", TypeIds.Int64Type);

            AssertDeclType("Byte", TypeIds.ByteType);
            AssertDeclType("Word", TypeIds.WordType);
            AssertDeclType("Cardinal", TypeIds.CardinalType);
            AssertDeclType("UInt64", TypeIds.Uint64Type);
            AssertDeclType("ShortInt", TypeIds.ShortInt);
            AssertDeclType("SmallInt", TypeIds.SmallInt);
            AssertDeclType("Integer", TypeIds.IntegerType);
            AssertDeclType("Int64", TypeIds.Int64Type);
        }

        [Fact]
        public void TestCharTypes() {
            AssertDeclType("System.Char", TypeIds.CharType);
            AssertDeclType("System.AnsiChar", TypeIds.AnsiCharType);
            AssertDeclType("System.WideChar", TypeIds.WideCharType);
            AssertDeclType("System.UCS2Char", TypeIds.Ucs2CharType);
            AssertDeclType("System.UCS4Char", TypeIds.Ucs4CharType);

            AssertDeclType("Char", TypeIds.CharType);
            AssertDeclType("AnsiChar", TypeIds.AnsiCharType);
            AssertDeclType("WideChar", TypeIds.WideCharType);
            AssertDeclType("UCS2Char", TypeIds.Ucs2CharType);
            AssertDeclType("UCS4Char", TypeIds.Ucs4CharType);
        }

        [Fact]
        public void TestBooleanTypes() {
            AssertDeclType("Boolean", TypeIds.BooleanType);
            AssertDeclType("ByteBool", TypeIds.ByteBoolType);
            AssertDeclType("WordBool", TypeIds.WordBoolType);
            AssertDeclType("LongBool", TypeIds.LongBoolType);

            AssertDeclType("System.Boolean", TypeIds.BooleanType);
            AssertDeclType("System.ByteBool", TypeIds.ByteBoolType);
            AssertDeclType("System.WordBool", TypeIds.WordBoolType);
            AssertDeclType("System.LongBool", TypeIds.LongBoolType);
        }

    }
}
