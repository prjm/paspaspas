using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Parsing.Tokenizer.TokenGroups;
using PasPasPasTests.Common;

namespace PasPasPasTests.Tokenizer {

    public class TokenizerPatternTests : CommonTest {

        private const string TestFileName = "test_file_name.pas";

        protected static IList<Token> RunTestTokenizer(string input) {
            var api = new TokenizerApi(CreateEnvironment());
            var result = new List<Token>();

            using (var tokenizer = api.CreateTokenizerForString(TestFileName, input)) {
                while (!tokenizer.AtEof) {
                    var token = tokenizer.CurrentToken;
                    Assert.IsNotNull(token);
                    result.Add(token);
                    tokenizer.FetchNextToken();
                }
            }

            return result;
        }


        [TestMethod]
        public void SimpleTests() {
            Assert.AreEqual(0, RunTestTokenizer(string.Empty).Count);
            Assert.AreEqual(1, RunTestTokenizer(" \n\n  ").Count);
            //Assert.AreEqual(3, RunTestTokenizer(" \n\n  ")[0].EndPosition.Line);
        }

        [TestMethod]
        public void TestSimpleCharClass() {
            var cc = new SingleCharClass('x');
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsFalse(cc.Matches('y'));
            Assert.IsFalse(cc.Matches('\0'));
        }

        [TestMethod]
        public void TestControlCharClass() {
            var cc = new ControlCharacterClass();
            Assert.IsTrue(cc.Matches('\a'));
            Assert.IsFalse(cc.Matches('\r'));
            Assert.IsFalse(cc.Matches('\n'));
        }

        [TestMethod]
        public void TestNumberCharClass() {
            var cc = new DigitCharClass(false);
            Assert.IsTrue(cc.Matches('0'));
            Assert.IsTrue(cc.Matches('1'));
            Assert.IsTrue(cc.Matches('2'));
            Assert.IsTrue(cc.Matches('3'));
            Assert.IsTrue(cc.Matches('4'));
            Assert.IsTrue(cc.Matches('5'));
            Assert.IsTrue(cc.Matches('7'));
            Assert.IsTrue(cc.Matches('8'));
            Assert.IsTrue(cc.Matches('9'));
            Assert.IsFalse(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        [TestMethod]
        public void TestIdentifierCharClass() {
            var cc = new IdentifierCharacterClass();
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsTrue(cc.Matches('X'));
            Assert.IsTrue(cc.Matches('&'));
            cc = new IdentifierCharacterClass(ampersands: false);
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsTrue(cc.Matches('X'));
            Assert.IsFalse(cc.Matches('&'));
            Assert.IsFalse(cc.Matches('1'));
            Assert.IsFalse(cc.Matches('.'));
            cc = new IdentifierCharacterClass(ampersands: false, dots: true);
            Assert.IsTrue(cc.Matches('X'));
            Assert.IsFalse(cc.Matches('&'));
            Assert.IsTrue(cc.Matches('.'));
            cc = new IdentifierCharacterClass(ampersands: false, digits: true);
            Assert.IsTrue(cc.Matches('0'));
            Assert.IsTrue(cc.Matches('9'));
            Assert.IsTrue(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        private const int PatternA = 1;
        private const int PatternB = 3;
        private readonly uint LogGuid = 99999;

        private IList<Token> RunTestPattern(InputPatterns patterns, Guid expectedMessage, string input) {
            var result = new List<Token>();
            var env = CreateEnvironment();
            using (var reader = ReaderApi.CreateReaderForString(TestFileName, input)) {
                var log = new LogSource(env.Log, LogGuid);
                var logTarget = new ListLogTarget();
                env.Log.RegisterTarget(logTarget);

                using (var tokenizer = new PasPasPas.Parsing.Tokenizer.TokenizerBase(env, patterns, reader)) {
                    while (reader.CurrentFile != null && !reader.AtEof) {
                        tokenizer.FetchNextToken();
                        result.Add(tokenizer.CurrentToken);
                    }
                }

                if (expectedMessage != Guid.Empty) {
                    Assert.AreEqual(1, logTarget.Messages.Count);
                    Assert.AreEqual(expectedMessage, logTarget.Messages[0].MessageID);
                }
                else {
                    Assert.AreEqual(0, logTarget.Messages.Count);
                }

                return result;
            }
        }

        public Token TestPattern(InputPatterns patterns, string input, params int[] tokenValues)
            => TestPattern(patterns, Guid.Empty, input, tokenValues);

        public Token TestPattern(InputPatterns patterns, Guid expectedMessage, string input, params int[] tokenValues) {
            var result = RunTestPattern(patterns, expectedMessage, input);
            var values = new List<string>();

            Assert.AreEqual(tokenValues.Length, result.Count);
            for (var i = 0; i < result.Count; i++) {
                Assert.AreEqual(tokenValues[i], result[i].Kind);
                values.Add(result[i].Value);
            }

            Assert.AreEqual(input, values.Count < 1 ? "" : values.Aggregate((a, b) => string.Concat(a, b)));

            if (result.Count > 0)
                return result[0];

            return Token.Empty;
        }

        [TestMethod]
        public void TestSimpleInputPatterns() {
            var patterns = new InputPatterns(null);
            TestPattern(patterns, "");
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "x", TokenKind.Invalid);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('b', PatternB);
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "aa", PatternA, PatternA);
            TestPattern(patterns, "b", PatternB);
        }

        [TestMethod]
        public void TestCurlyBraceCommentTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', new SequenceGroupTokenValue(TokenKind.Comment, "}"));
            TestPattern(patterns, "a{}", PatternA, TokenKind.Comment);
            TestPattern(patterns, "{}", TokenKind.Comment);
            TestPattern(patterns, "{}a", TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{//}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{(**)}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{{}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "a{", PatternA, TokenKind.Comment);
        }


        [TestMethod]
        public void TestAlternativeCurlyBraceCommentTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('(', TokenKind.OpenParen).Add('*', new SequenceGroupTokenValue(TokenKind.Comment, "*)"));
            TestPattern(patterns, "a(", PatternA, TokenKind.OpenParen);
            TestPattern(patterns, "(**)", TokenKind.Comment);
            TestPattern(patterns, "(*a*)", TokenKind.Comment);
            TestPattern(patterns, "(*(*(*(*a*)", TokenKind.Comment);
            TestPattern(patterns, "a(*a*)a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "(**)a", TokenKind.Comment, PatternA);
            TestPattern(patterns, "a(**)a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a(*\n\n*)a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a(*\n***())()()\n*)a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a(*{//}*)a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a(*(**)a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "a(*", PatternA, TokenKind.Comment);
        }

        [TestMethod]
        public void TestPreprocessorTokenVaue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', TokenKind.Comma).Add('$', new SequenceGroupTokenValue(TokenKind.Preprocessor, "}"));
            TestPattern(patterns, "a{${//}a", PatternA, TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, "a{${}a", PatternA, TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, "{${}a", TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "a{$", PatternA, TokenKind.Preprocessor);
        }


        [TestMethod]
        public void TestControlCharTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new ControlCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.ControlChar, new ControlCharacterClass()));
            TestPattern(patterns, "");
            TestPattern(patterns, "a\u0000\u0001\u0002\u0003\u0004", PatternA, TokenKind.ControlChar);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "\r", TokenKind.Invalid);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "\n", TokenKind.Invalid);
        }

        [TestMethod]
        public void TestWhitespaceCharTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            TestPattern(patterns, "");
            TestPattern(patterns, "a    a", PatternA, TokenKind.WhiteSpace, PatternA);
            TestPattern(patterns, "   ", TokenKind.WhiteSpace);
            TestPattern(patterns, "\t\r\n\r\f", TokenKind.WhiteSpace);
            TestPattern(patterns, "aa\na", PatternA, PatternA, TokenKind.WhiteSpace, PatternA);
        }

        [TestMethod]
        public void TestDigitTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new DigitCharClass(false), new CharacterClassTokenGroupValue(TokenKind.IntegralNumber, new DigitCharClass(false)));
            TestPattern(patterns, "1", TokenKind.IntegralNumber);
            TestPattern(patterns, "1234567890", TokenKind.IntegralNumber);
            TestPattern(patterns, "000", TokenKind.IntegralNumber);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "３", TokenKind.Invalid);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "1３", TokenKind.IntegralNumber, TokenKind.Invalid);
        }

        [TestMethod]
        public void TestHexNumberTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('$', new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteHexNumber));
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteHexNumber, "$", TokenKind.HexNumber);
            TestPattern(patterns, "$1234567890", TokenKind.HexNumber);
            TestPattern(patterns, "$ABCDEF", TokenKind.HexNumber);
            TestPattern(patterns, "$abcdef", TokenKind.HexNumber);
            TestPattern(patterns, "$000000", TokenKind.HexNumber);
            TestPattern(patterns, "$1234FFFF", TokenKind.HexNumber);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "$CEFO", TokenKind.HexNumber, TokenKind.Invalid);
            Assert.AreEqual(GetIntegerValue(0x123F), TestPattern(patterns, "$123F", TokenKind.HexNumber).ParsedValue);
        }

        private InputPatterns CreatePatterns(bool allowAmpersand = true, bool allowDigits = false, bool allowDot = false) {
            var patterns = new InputPatterns(null);
            var tokens = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["a"] = PatternA
            };
            var tgv = new IdentifierTokenGroupValue(tokens, allowAmpersand, allowDigits, allowDot);
            patterns.AddPattern('b', PatternB);
            patterns.AddPattern(new IdentifierCharacterClass(), tgv);
            patterns.AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            return patterns;
        }

        [TestMethod]
        public void TestIdentifierTokenValue() {
            var patterns = CreatePatterns();
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "&a", TokenKind.Identifier);
            TestPattern(patterns, "_a", TokenKind.Identifier);
            TestPattern(patterns, "画像", TokenKind.Identifier);
            TestPattern(patterns, "a b caaa", PatternA, TokenKind.WhiteSpace, PatternB, TokenKind.WhiteSpace, TokenKind.Identifier);
            patterns = CreatePatterns(false);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "&a", TokenKind.Invalid, PatternA);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "a9", PatternA, TokenKind.Invalid);
            patterns = CreatePatterns(false, true);
            TestPattern(patterns, "a9", TokenKind.Identifier);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedCharacter, "a.a9", PatternA, TokenKind.Invalid, TokenKind.Identifier);
            TestPattern(patterns, "_a_aaA123_33", TokenKind.Identifier);
            patterns = CreatePatterns(false, true, true);
            TestPattern(patterns, "a.9", TokenKind.Identifier);
        }

        [TestMethod]
        public void TestEndOfLineCommentTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('/', TokenKind.Slash).Add('/', new EndOfLineCommentTokenGroupValue());
            patterns.AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            TestPattern(patterns, "/", TokenKind.Slash);
            TestPattern(patterns, "//", TokenKind.Comment);
            TestPattern(patterns, "// / / /", TokenKind.Comment);
            TestPattern(patterns, "/// / / /", TokenKind.Comment);
            TestPattern(patterns, "/ // / / /", TokenKind.Slash, TokenKind.WhiteSpace, TokenKind.Comment);
            TestPattern(patterns, "/ // / /\n /", TokenKind.Slash, TokenKind.WhiteSpace, TokenKind.Comment, TokenKind.WhiteSpace, TokenKind.Slash);
        }

        [TestMethod]
        public void TestNumberTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern(new DigitCharClass(false), new NumberTokenGroupValue());
            patterns.AddPattern(new IdentifierCharacterClass(), new IdentifierTokenGroupValue(new Dictionary<string, int>()));
            patterns.AddPattern('.', TokenKind.Dot).Add('.', TokenKind.DotDot);
            TestPattern(patterns, "9..", TokenKind.IntegralNumber, TokenKind.DotDot);
            TestPattern(patterns, "9", TokenKind.IntegralNumber);
            TestPattern(patterns, "9.9", TokenKind.RealNumber);
            TestPattern(patterns, "9999.9999", TokenKind.RealNumber);
            TestPattern(patterns, "9.", TokenKind.IntegralNumber, TokenKind.Dot);
            TestPattern(patterns, "9.X", TokenKind.IntegralNumber, TokenKind.Dot, TokenKind.Identifier);
            TestPattern(patterns, "9.9.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "9.9e", TokenKind.RealNumber);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "9.9e+", TokenKind.RealNumber);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "9.9e-", TokenKind.RealNumber);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "9.9e.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "9.9e+.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.UnexpectedEndOfToken, "9.9e-.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, "9999.9999E+3", TokenKind.RealNumber);
            TestPattern(patterns, "9999.9999E-3", TokenKind.RealNumber);
            TestPattern(patterns, "9999.9999E-3.", TokenKind.RealNumber, TokenKind.Dot);
        }

        [TestMethod]
        public void TestDoubleQuotedStringTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('"', new QuotedStringTokenValue(TokenKind.DoubleQuotedString, '"'));
            TestPattern(patterns, "\"aaaaaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"aaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"\"\"aaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteString, "\"aaaaaa", TokenKind.DoubleQuotedString);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteString, "\"", TokenKind.DoubleQuotedString);
        }

        [TestMethod]
        public void TestQuotedStringTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('\'', new QuotedStringTokenValue(TokenKind.QuotedString, '\''));
            TestPattern(patterns, "'aaaaaa'", TokenKind.QuotedString);
            TestPattern(patterns, "'aaa''aaa'", TokenKind.QuotedString);
            TestPattern(patterns, "'aaa''''aaa'", TokenKind.QuotedString);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteString, "'aaaaaa", TokenKind.QuotedString);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteString, "'", TokenKind.QuotedString);
            Assert.AreEqual(GetWideCharValue('a'), TestPattern(patterns, "'a'", TokenKind.QuotedString).ParsedValue);
            Assert.AreEqual(GetUnicodeStringValue("a'"), TestPattern(patterns, "'a'''", TokenKind.QuotedString).ParsedValue);
            Assert.AreEqual(GetUnicodeStringValue("a'aa"), TestPattern(patterns, "'a''aa'", TokenKind.QuotedString).ParsedValue);
        }

        [TestMethod]
        public void TestStringGroupTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('#', new StringGroupTokenValue());
            patterns.AddPattern('\'', new StringGroupTokenValue());
            Assert.AreEqual(GetUnicodeStringValue("a\nb"), TestPattern(patterns, "'a'#$A'b'", TokenKind.QuotedString).ParsedValue);
            TestPattern(patterns, "'aaa'", TokenKind.QuotedString);
            TestPattern(patterns, "#09", TokenKind.QuotedString);
            TestPattern(patterns, "#09'aaaa'", TokenKind.QuotedString);
            TestPattern(patterns, "#09'aaaa'#09", TokenKind.QuotedString);
            TestPattern(patterns, "#$09'aaaa'#09", TokenKind.QuotedString);
            TestPattern(patterns, "#$09'aaaa'#$09", TokenKind.QuotedString);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteString, "#", TokenKind.QuotedString);
            TestPattern(patterns, PasPasPas.Parsing.Tokenizer.TokenizerBase.IncompleteString, "#$", TokenKind.QuotedString);
            TestPattern(patterns, "'aaaa'#$09", TokenKind.QuotedString);
        }

    }
}
