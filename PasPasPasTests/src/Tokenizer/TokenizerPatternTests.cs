using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Parsing.Tokenizer.TokenGroups;
using PasPasPasTests.Common;

namespace PasPasPasTests.Tokenizer {

    /// <summary>
    ///     test token patterns
    /// </summary>
    public class TokenizerPatternTests : CommonTest {

        private const string TestFileName = "test_file_name.pas";

        /// <summary>
        ///     run a tokenizer test
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected static IList<Token> RunTestTokenizer(string input) {
            var env = CreateEnvironment();
            var path = env.CreateFileReference(TestFileName);
            var resolver = CommonApi.CreateResolverForSingleString(path, input);
            var options = Factory.CreateOptions(resolver, env);
            var api = Factory.CreateTokenizerApi(options);
            var result = new List<Token>();

            using (var tokenizer = api.CreateTokenizer(resolver, path)) {
                while (!tokenizer.AtEof) {
                    var token = tokenizer.CurrentToken;
                    Assert.IsNotNull(token);
                    result.Add(token);
                    tokenizer.FetchNextToken();
                }
            }

            return result;
        }

        /// <summary>
        ///     example tests
        /// </summary>
        [TestMethod]
        public void SimpleTests() {
            Assert.AreEqual(0, RunTestTokenizer(string.Empty).Count);
            Assert.AreEqual(1, RunTestTokenizer(" \n\n  ").Count);
            //Assert.AreEqual(3, RunTestTokenizer(" \n\n  ")[0].EndPosition.Line);
        }

        /// <summary>
        ///     test char matching
        /// </summary>
        [TestMethod]
        public void TestSimpleCharClass() {
            var cc = new SingleCharClass('x');
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsFalse(cc.Matches('y'));
            Assert.IsFalse(cc.Matches('\0'));
        }

        /// <summary>
        ///     test control characters
        /// </summary>
        [TestMethod]
        public void TestControlCharClass() {
            var cc = new ControlCharacterClass();
            Assert.IsTrue(cc.Matches('\a'));
            Assert.IsFalse(cc.Matches('\r'));
            Assert.IsFalse(cc.Matches('\n'));
        }

        /// <summary>
        ///     test number matching
        /// </summary>
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

        /// <summary>
        ///     test identifier matching
        /// </summary>
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

        private IList<Token> RunTestPattern(InputPatterns patterns, uint expectedMessage, string input) {
            var result = new List<Token>();
            var env = CreateEnvironment();
            var api = Factory.CreateReaderApi(env);
            var file = env.CreateFileReference(TestFileName);
            var data = CreateResolver(file, input);

            using (var reader = api.CreateReader(data, file)) {
                var log = env.Log.CreateLogSource(LogGuid);
                var logTarget = new ListLogTarget();
                env.Log.RegisterTarget(logTarget);

                using (var tokenizer = new PasPasPas.Parsing.Tokenizer.TokenizerBase(env, patterns, reader)) {
                    while (reader.CurrentFile != null && !reader.AtEof) {
                        tokenizer.FetchNextToken();
                        result.Add(tokenizer.CurrentToken);
                    }
                }

                if (expectedMessage != 0) {
                    Assert.AreEqual(1, logTarget.Messages.Count);
                    Assert.AreEqual(expectedMessage, logTarget.Messages[0].MessageID);
                }
                else {
                    Assert.AreEqual(0, logTarget.Messages.Count);
                }

                return result;
            }
        }

        /// <summary>
        ///     pattern test helper
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="input"></param>
        /// <param name="tokenValues"></param>
        /// <returns></returns>
        public Token TestPattern(InputPatterns patterns, string input, params int[] tokenValues)
            => TestPattern(patterns, 0, input, tokenValues);

        /// <summary>
        ///     test a pattern
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="expectedMessage"></param>
        /// <param name="input"></param>
        /// <param name="tokenValues"></param>
        /// <returns></returns>
        public Token TestPattern(InputPatterns patterns, uint expectedMessage, string input, params int[] tokenValues) {
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

        /// <summary>
        ///     test simple input patterns
        /// </summary>
        [TestMethod]
        public void TestSimpleInputPatterns() {
            var patterns = new InputPatterns(null);
            TestPattern(patterns, "");
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "x", TokenKind.Invalid);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('b', PatternB);
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "aa", PatternA, PatternA);
            TestPattern(patterns, "b", PatternB);
        }

        /// <summary>
        ///     test curly brace comments
        /// </summary>
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
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "a{", PatternA, TokenKind.Comment);
        }

        /// <summary>
        ///     test combined brace comments
        /// </summary>
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
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "a(*", PatternA, TokenKind.Comment);
        }

        /// <summary>
        ///     test preprocessor tokens
        /// </summary>
        [TestMethod]
        public void TestPreprocessorTokenVaue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', TokenKind.Comma).Add('$', new SequenceGroupTokenValue(TokenKind.Preprocessor, "}"));
            TestPattern(patterns, "a{${//}a", PatternA, TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, "a{${}a", PatternA, TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, "{${}a", TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "a{$", PatternA, TokenKind.Preprocessor);
        }

        /// <summary>
        ///     test control chars
        /// </summary>
        [TestMethod]
        public void TestControlCharTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new ControlCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.ControlChar, new ControlCharacterClass()));
            TestPattern(patterns, "");
            TestPattern(patterns, "a\u0000\u0001\u0002\u0003\u0004", PatternA, TokenKind.ControlChar);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "\r", TokenKind.Invalid);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "\n", TokenKind.Invalid);
        }

        /// <summary>
        ///     test whitespace token values
        /// </summary>
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

        /// <summary>
        ///     test digit matching
        /// </summary>
        [TestMethod]
        public void TestDigitTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new DigitCharClass(false), new CharacterClassTokenGroupValue(TokenKind.IntegralNumber, new DigitCharClass(false)));
            TestPattern(patterns, "1", TokenKind.IntegralNumber);
            TestPattern(patterns, "1234567890", TokenKind.IntegralNumber);
            TestPattern(patterns, "000", TokenKind.IntegralNumber);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "３", TokenKind.Invalid);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "1３", TokenKind.IntegralNumber, TokenKind.Invalid);
        }

        /// <summary>
        ///     test hex numbers
        /// </summary>
        [TestMethod]
        public void TestHexNumberTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('$', new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, MessageNumbers.IncompleteHexNumber));
            TestPattern(patterns, MessageNumbers.IncompleteHexNumber, "$", TokenKind.HexNumber);
            TestPattern(patterns, "$1234567890", TokenKind.HexNumber);
            TestPattern(patterns, "$ABCDEF", TokenKind.HexNumber);
            TestPattern(patterns, "$abcdef", TokenKind.HexNumber);
            TestPattern(patterns, "$000000", TokenKind.HexNumber);
            TestPattern(patterns, "$1234FFFF", TokenKind.HexNumber);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "$CEFO", TokenKind.HexNumber, TokenKind.Invalid);
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

        /// <summary>
        ///     test identifier tokens
        /// </summary>
        [TestMethod]
        public void TestIdentifierTokenValue() {
            var patterns = CreatePatterns();
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "&a", TokenKind.Identifier);
            TestPattern(patterns, "_a", TokenKind.Identifier);
            TestPattern(patterns, "画像", TokenKind.Identifier);
            TestPattern(patterns, "a b caaa", PatternA, TokenKind.WhiteSpace, PatternB, TokenKind.WhiteSpace, TokenKind.Identifier);
            patterns = CreatePatterns(false);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "&a", TokenKind.Invalid, PatternA);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "a9", PatternA, TokenKind.Invalid);
            patterns = CreatePatterns(false, true);
            TestPattern(patterns, "a9", TokenKind.Identifier);
            TestPattern(patterns, MessageNumbers.UnexpectedCharacter, "a.a9", PatternA, TokenKind.Invalid, TokenKind.Identifier);
            TestPattern(patterns, "_a_aaA123_33", TokenKind.Identifier);
            patterns = CreatePatterns(false, true, true);
            TestPattern(patterns, "a.9", TokenKind.Identifier);
        }

        /// <summary>
        ///     test end of line comments
        /// </summary>
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

        /// <summary>
        ///     test numeric tokens
        /// </summary>
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
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "9.9e", TokenKind.RealNumber);
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "9.9e+", TokenKind.RealNumber);
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "9.9e-", TokenKind.RealNumber);
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "9.9e.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "9.9e+.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, MessageNumbers.UnexpectedEndOfToken, "9.9e-.", TokenKind.RealNumber, TokenKind.Dot);
            TestPattern(patterns, "9999.9999E+3", TokenKind.RealNumber);
            TestPattern(patterns, "9999.9999E-3", TokenKind.RealNumber);
            TestPattern(patterns, "9999.9999E-3.", TokenKind.RealNumber, TokenKind.Dot);
        }

        /// <summary>
        ///     test double quoted strings
        /// </summary>
        [TestMethod]
        public void TestDoubleQuotedStringTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('"', new QuotedStringTokenValue(TokenKind.DoubleQuotedString, '"'));
            TestPattern(patterns, "\"aaaaaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"aaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"\"\"aaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, MessageNumbers.IncompleteString, "\"aaaaaa", TokenKind.DoubleQuotedString);
            TestPattern(patterns, MessageNumbers.IncompleteString, "\"", TokenKind.DoubleQuotedString);
        }

        /// <summary>
        ///     test quoted strings
        /// </summary>
        [TestMethod]
        public void TestQuotedStringTokenValue() {
            var patterns = new InputPatterns(null);
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('\'', new QuotedStringTokenValue(TokenKind.QuotedString, '\''));
            TestPattern(patterns, "'aaaaaa'", TokenKind.QuotedString);
            TestPattern(patterns, "'aaa''aaa'", TokenKind.QuotedString);
            TestPattern(patterns, "'aaa''''aaa'", TokenKind.QuotedString);
            TestPattern(patterns, MessageNumbers.IncompleteString, "'aaaaaa", TokenKind.QuotedString);
            TestPattern(patterns, MessageNumbers.IncompleteString, "'", TokenKind.QuotedString);
            Assert.AreEqual(GetWideCharValue('a'), TestPattern(patterns, "'a'", TokenKind.QuotedString).ParsedValue);
            Assert.AreEqual(GetUnicodeStringValue("a'"), TestPattern(patterns, "'a'''", TokenKind.QuotedString).ParsedValue);
            Assert.AreEqual(GetUnicodeStringValue("a'aa"), TestPattern(patterns, "'a''aa'", TokenKind.QuotedString).ParsedValue);
        }

        /// <summary>
        ///     test string tokenizer
        /// </summary>
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
            TestPattern(patterns, MessageNumbers.IncompleteString, "#", TokenKind.QuotedString);
            TestPattern(patterns, MessageNumbers.IncompleteString, "#$", TokenKind.QuotedString);
            TestPattern(patterns, "'aaaa'#$09", TokenKind.QuotedString);
        }

    }
}
