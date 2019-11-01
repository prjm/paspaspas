using System;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Runtime.Values.BooleanValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPasTests.Common;

namespace PasPasPasTests.Tokenizer {

    /// <summary>
    ///     simple tokenizer tests
    /// </summary>
    public class SimpleTokenizerTests : TokenizerTest {

        /// <summary>
        ///     test identifiers
        /// </summary>
        [TestMethod]
        public void TestIdentifiers() {
            IsIdentifier("abcd");
            IsIdentifier("ABCD");
            IsIdentifier("öäöäö");
            IsIdentifier("_dummy");
            IsIdentifier("du_mmy");
            IsIdentifier("dummy_");
            IsIdentifier("&program");
            IsIdentifier("asd994");
        }

        /// <summary>
        ///     test integers
        /// </summary>
        [TestMethod]
        public void TestIntegers() {
            IsInteger("2", 2);
            IsInteger("123", 123);
            IsInteger("0000", 0);
            IsInteger("10000", 10000);
            IsInteger("300", 300);
            IsInteger("1000000", 1000000);
            IsInteger("18446744073709551615", 18446744073709551615);
            IsInteger("18446744073709551616", new IntegerOperations(new BooleanOperations(), null).Overflow);
            IsInteger("108446744073709551615", new IntegerOperations(new BooleanOperations(), null).Overflow);
        }

        /// <summary>
        ///     test whitespace
        /// </summary>
        [TestMethod]
        public void TestWhitespace() {
            IsWhitespace(" ");
            IsWhitespace(" " + System.Environment.NewLine);
            IsWhitespace("  ");
        }

        /// <summary>
        ///     test real numbers
        /// </summary>
        [TestMethod]
        public void TestRealNumbers() {
            IsReal("123E10", "123E10");
            IsReal("123.", "123", Tuple.Create(TokenKind.IntegralNumber, "123"), Tuple.Create(TokenKind.Dot, "."));
            IsReal("123.123", "123.123");
            IsReal("123E+10", "123E10");
            IsReal("123E-10", "123E-10");
            IsReal("123.123E-10", "123.123E-10");
            IsReal("123.123E+10", "123.123E+10");
        }

        /// <summary>
        ///     test preprocessor commands
        /// </summary>
        [TestMethod]
        public void TestIsPreprocessorCommand() {
            IsPreprocessor("{$}", "");
            IsPreprocessor("{$ }", " ");
            IsPreprocessor("{$A}", "A");
            IsPreprocessor("(*$HPPEMIT '}'*)", "HPPEMIT '}'");
        }

        /// <summary>
        ///     test different comments
        /// </summary>
        [TestMethod]
        public void TestComment() {
            IsComment("// dsfssdf ");
            IsComment("{ dsfssdf }");
            IsComment("(* HPPEMIT '}'*)");
        }

        /// <summary>
        ///     test control characters
        /// </summary>
        [TestMethod]
        public void TestControlChars() {
            IsControlChar("\u0000");
            IsControlChar("\u0001");
            IsControlChar("\u0002");
            IsControlChar("\u0003");
            IsControlChar("\u0004");
            IsControlChar("\u0005");
            IsControlChar("\u0006");
            IsControlChar("\u0007");
            IsControlChar("\u0008");
            IsControlChar("\u000E");
            IsControlChar("\u000F");
            IsControlChar("\u0010");
            IsControlChar("\u0011");
            IsControlChar("\u0012");
            IsControlChar("\u0013");
            IsControlChar("\u0014");
            IsControlChar("\u0015");
            IsControlChar("\u0016");
            IsControlChar("\u0017");
            IsControlChar("\u0018");
            IsControlChar("\u0019");
            IsControlChar("\u001B");
            IsControlChar("\u001C");
            IsControlChar("\u001D");
            IsControlChar("\u001E");
            IsControlChar("\u001F");
        }

        /// <summary>
        ///     test simple tokens
        /// </summary>
        [TestMethod]
        public void TestSimpleTokens() {
            IsToken(TokenKind.Comma, ",");
            IsToken(TokenKind.Dot, ".");
            IsToken(TokenKind.DotDot, "..");
            IsToken(TokenKind.OpenParen, "(");
            IsToken(TokenKind.CloseParen, ")");
            IsToken(TokenKind.OpenBraces, "(.");
            IsToken(TokenKind.CloseBraces, ".)");
            IsToken(TokenKind.Comment, "(* xx *)");
            IsToken(TokenKind.Semicolon, ";");
            IsToken(TokenKind.EqualsSign, "=");
            IsToken(TokenKind.Assignment, ":=");
            IsToken(TokenKind.Colon, ":");
            IsToken(TokenKind.Circumflex, "^");
            IsToken(TokenKind.Plus, "+");
            IsToken(TokenKind.Minus, "-");
            IsToken(TokenKind.Times, "*");
            IsToken(TokenKind.Slash, "/");
            IsToken(TokenKind.Comment, "// xxx");
            IsToken(TokenKind.At, "@");
            IsToken(TokenKind.LessThen, "<");
            IsToken(TokenKind.LessThenEquals, "<=");
            IsToken(TokenKind.GreaterThen, ">");
            IsToken(TokenKind.GreaterThenEquals, ">=");
            IsToken(TokenKind.NotEquals, "<>");
            IsToken(TokenKind.Comment, "{ ddd }");
            IsToken(TokenKind.Preprocessor, "{$ ddd }", "{$ ddd }", GetUnicodeStringValue(" ddd "));
            IsToken(TokenKind.WhiteSpace, "  ");
            IsToken(TokenKind.HexNumber, "$0000", "$0000", GetIntegerValue((sbyte)0));
            IsToken(TokenKind.DoubleQuotedString, "\"\"", "\"\"", GetUnicodeStringValue(string.Empty));
        }

        /// <summary>
        ///     test hex numbers
        /// </summary>
        [TestMethod]
        public void TestHexNumbers() {
            IsHexNumber("$333F", GetIntegerValue((short)0x333F));
            IsHexNumber("$000000", GetIntegerValue((sbyte)0));
            IsHexNumber("$FFFFFFFFFFFFFFFF", GetIntegerValue(0xFFFFFFFFFFFFFFFF));
        }

        /// <summary>
        ///     test quoted strings
        /// </summary>
        [TestMethod]
        public void TestQuotedString() {
            IsQuotedString("''", string.Empty);
            IsQuotedString("'sdfddfsd'", "sdfddfsd");
            IsQuotedString("'sdf''ddfsd'", "sdf'ddfsd");
            IsWideChar("#45", '-');
            IsQuotedString("#45'xxx'#55", "-xxx7");
            IsQuotedString("'ddd'#45'ddd-xxx'", "ddd-ddd-xxx");
            IsWideChar("#$58D", '֍');
            IsQuotedString("'ddd'#$58D'xxx'", "ddd֍xxx");
        }

        /// <summary>
        ///     test messages
        /// </summary>
        [TestMethod]
        public void TestMessages() {
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteHexNumber, "$");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteIdentifier, "&");
            TokenizerMessageIsGenerated(MessageNumbers.UnexpectedCharacter, "´");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteString, "'");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteString, "\"");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteString, "  '");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteString, "  '   ");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteString, "#");
            TokenizerMessageIsGenerated(MessageNumbers.UnexpectedCharacter, "#D");
            TokenizerMessageIsGenerated(MessageNumbers.IncompleteString, "#$R");
        }

    }
}
