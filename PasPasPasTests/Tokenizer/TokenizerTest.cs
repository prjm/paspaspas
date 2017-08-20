using System.Collections.Generic;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer;
using Xunit;
using System.Linq;
using System;

namespace PasPasPasTests.Tokenizer {

    public class TokenizerTest {

        [Fact]
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

        [Fact]
        public void TestIntegers() {
            IsInteger("123");
            IsInteger("0000");
            IsInteger("10000");
        }

        [Fact]
        public void TestWhitespace() {
            IsWhitespace(" ");
            IsWhitespace(" " + System.Environment.NewLine);
            IsWhitespace("  ");
        }

        [Fact]
        public void TestRealNumbers() {
            IsReal("123E10");
            IsReal("123.", Tuple.Create(TokenKind.Integer, "123"), Tuple.Create(TokenKind.Dot, "."), Tuple.Create(TokenKind.Eof, string.Empty));
            IsReal("123.123");
            IsReal("123E+10");
            IsReal("123E-10");
            IsReal("123.123E-10");
            IsReal("123.123E+10");
        }

        [Fact]
        public void TestIsPreprocessorCommand() {
            IsPreprocessor("{$A}");
            IsPreprocessor("(*$HPPEMIT '}'*)");
        }

        [Fact]
        public void TestComment() {
            IsComment("// dsfssdf ");
            IsComment("{ dsfssdf }");
            IsComment("(* HPPEMIT '}'*)");
        }

        [Fact]
        public void TestHexNumbers() {
            IsHexNumber("$333F");
            IsHexNumber("$000000");
        }

        [Fact]
        public void TestQuotedString() {
            IsQuotedString("''");
            IsQuotedString("'sdfddfsd'");
            IsQuotedString("'sdf''ddfsd'");
            IsQuotedString("#45");
            IsQuotedString("#45'xxx'#55");
            IsQuotedString("'ddd'#45'xxx'");
            IsQuotedString("#$F45");
            IsQuotedString("'ddd'#$1245'xxx'");
        }

        [Fact]
        public void TestMessages() {
            TokenizerMessageIsGenerated(StandardTokenizer.IncompleteHexNumber, "$");
            TokenizerMessageIsGenerated(StandardTokenizer.IncompleteIdentifier, "&");
            TokenizerMessageIsGenerated(TokenizerBase.UnexpectedCharacter, "´");
        }

        public static void IsQuotedString(string input)
            => IsToken(TokenKind.QuotedString, input, input);

        public static void IsInteger(string input)
            => IsToken(TokenKind.Integer, input, input);

        public static void IsWhitespace(string input)
            => IsToken(TokenKind.WhiteSpace, input, input);

        public static void IsReal(string input, params Tuple<int, string>[] tokens)
            => IsToken(TokenKind.Real, input, input, tokens);

        public static void IsHexNumber(string input)
            => IsToken(TokenKind.HexNumber, input, input);

        public static void IsPreprocessor(string input)
            => IsToken(TokenKind.Preprocessor, input, input);

        public static void IsComment(string input)
            => IsToken(TokenKind.Comment, input, input);

        public static void IsAssembler(string input)
            => IsToken(TokenKind.Asm, input, input);

        public static void IsIdentifier(string input, string output = null) {
            if (output == null)
                output = input;

            IsToken(TokenKind.Identifier, output, input);
        }

        public static void IsToken(int tokenKind, string tokenValue, string input, params Tuple<int, string>[] tokens) {
            var result = TestHelper.RunTokenizer(input);
            Assert.IsNotNull(result);

            if (tokens.Length < 1) {
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(tokenKind, result[0].Kind);
                Assert.AreEqual(tokenValue, result[0].Value);
                Assert.AreEqual(TokenKind.Eof, result[1].Kind);
            }
            else {
                Assert.AreEqual(tokens.Length, result.Count);
                for (var index = 0; index < tokens.Length; index++) {
                    Assert.AreEqual(tokens[index].Item1, result[index].Kind);
                    Assert.AreEqual(tokens[index].Item2, result[index].Value);
                }
            }
        }

        internal static void TokenizerMessageIsGenerated(Guid messageNumber, string input) {
            var messages = new List<ILogMessage>();
            var result = TestHelper.RunTokenizer(input, messages);
            var hasMessage = messages.Any(t => t.MessageID == messageNumber);
            Assert.IsTrue(hasMessage);
        }



    }
}
