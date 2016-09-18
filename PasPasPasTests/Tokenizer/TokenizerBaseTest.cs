using PasPasPas.Parsing.Tokenizer;
using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Parser;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using Xunit;

namespace PasPasPasTests.Tokenizer {


    internal class TestTokenizer : TokenizerBase {

        private readonly InputPatterns puncts;

        public TestTokenizer(ParserServices environment, StackedFileReader input)
            : base(environment, input) {
            puncts = new InputPatterns();
            puncts.AddPattern(new WhiteSpaceCharacterClass(), new WhiteSpaceTokenGroupValue());
        }

        protected override InputPatterns CharacterClasses
            => puncts;
    }

    public class TokenizerBaseTest {

        private const string TestFileName = "test_file_name.pas";

        protected IList<Token> RunTestTokenizer(string input) {
            var result = new List<Token>();
            var log = new LogManager();
            var environment = new ParserServices(log);

            using (var inputFile = new StringInput(input, new FileReference(TestFileName)))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                var tokenizer = new TestTokenizer(environment, reader);

                while (!reader.AtEof) {
                    var token = tokenizer.FetchNextToken();
                    Assert.IsNotNull(token);
                    result.Add(token);
                }
            }


            return result;
        }


        [Fact]
        public void SimpleTests() {
            Assert.AreEqual(0, RunTestTokenizer(string.Empty).Count);
            Assert.AreEqual(1, RunTestTokenizer(" \n\n  ").Count);
            Assert.AreEqual(3, RunTestTokenizer(" \n\n  ")[0].EndPosition.Line);
        }

        [Fact]
        public void TestSimpleCharClass() {
            SingleCharClass cc = new SingleCharClass('x');
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsFalse(cc.Matches('y'));
            Assert.IsFalse(cc.Matches('\0'));
        }

        [Fact]
        public void TestControlCharClass() {
            ControlCharacterClass cc = new ControlCharacterClass();
            Assert.IsTrue(cc.Matches('\a'));
            Assert.IsFalse(cc.Matches('\r'));
            Assert.IsFalse(cc.Matches('\n'));
        }

        [Fact]
        public void TestNumberCharClass() {
            NumberCharacterClass cc = new NumberCharacterClass();
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

        [Fact]
        public void TestExponentCharClass() {
            var cc = new ExponentCharacterClass();
            Assert.IsTrue(cc.Matches('E'));
            Assert.IsTrue(cc.Matches('e'));
            Assert.IsFalse(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        [Fact]
        public void TestPlusMinusCharClass() {
            var cc = new PlusMinusCharacterClass();
            Assert.IsTrue(cc.Matches('+'));
            Assert.IsTrue(cc.Matches('-'));
            Assert.IsFalse(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        [Fact]
        public void TestIdentifierCharClass() {
            var cc = new IdentifierCharacterClass();
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsTrue(cc.Matches('X'));
            Assert.IsTrue(cc.Matches('&'));
            cc.AllowAmpersand = false;
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsTrue(cc.Matches('X'));
            Assert.IsFalse(cc.Matches('&'));
            Assert.IsFalse(cc.Matches('1'));
            Assert.IsFalse(cc.Matches('.'));
            cc.AllowDots = true;
            Assert.IsTrue(cc.Matches('X'));
            Assert.IsFalse(cc.Matches('&'));
            Assert.IsTrue(cc.Matches('.'));
            cc.AllowDigits = true;
            Assert.IsTrue(cc.Matches('0'));
            Assert.IsTrue(cc.Matches('9'));
            Assert.IsTrue(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        private const int PatternA = 1;
        private const int PatternAA = 2;
        private const int PatternB = 3;
        private readonly Guid LogGuid = new Guid("{7FD95F57-A165-4736-A2BF-BC3EE2C30F5F}");

        private IList<Token> RunTestPattern(InputPatterns patterns, Guid expectedMessage, string input) {
            var result = new List<Token>();
            var manager = new LogManager();
            var log = new LogSource(manager, LogGuid);
            var logTarget = new ListLogTarget();
            manager.RegisterTarget(logTarget);
            using (StringInput inputFile = new StringInput(input, new FileReference(TestFileName)))
            using (StackedFileReader reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                while (!reader.AtEof) {
                    result.Add(patterns.FetchNextToken(reader, log));
                }

                if (expectedMessage != Guid.Empty) {
                    Assert.AreEqual(1, logTarget.Messages.Count);
                    Assert.AreEqual(expectedMessage, logTarget.Messages[0].MessageID);
                }
                else {
                    Assert.AreEqual(0, logTarget.Messages.Count);
                }

                return result;
            };
        }

        public Token TestPattern(InputPatterns patterns, string input, params int[] tokenValues)
            => TestPattern(patterns, Guid.Empty, input, tokenValues);

        public Token TestPattern(InputPatterns patterns, Guid expectedMessage, string input, params int[] tokenValues) {
            IList<Token> result = RunTestPattern(patterns, expectedMessage, input);
            Assert.AreEqual(tokenValues.Length, result.Count);
            for (int i = 0; i < result.Count; i++)
                Assert.AreEqual(tokenValues[i], result[i].Kind);

            if (result.Count > 0)
                return result[0];

            return null;
        }

        [Fact]
        public void TestSimpleInputPatterns() {
            var patterns = new InputPatterns();
            TestPattern(patterns, "");
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "x", TokenKind.Undefined);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('b', PatternB);
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "aa", PatternA, PatternA);
            TestPattern(patterns, "b", PatternB);
        }

        [Fact]
        public void TestCurlyBraceCommentTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', new CurlyBraceCommentTokenValue());
            TestPattern(patterns, "a{}", PatternA, TokenKind.Comment);
            TestPattern(patterns, "{}", TokenKind.Comment);
            TestPattern(patterns, "{}a", TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{//}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{(**)}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, "a{{}a", PatternA, TokenKind.Comment, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "a{", PatternA, TokenKind.Comment);
        }


        [Fact]
        public void TestAlternativeCurlyBraceCommentTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('(', TokenKind.OpenParen).Add('*', new AlternativeCurlyBraceCommentTokenValue());
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
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "a(*", PatternA, TokenKind.Comment);
        }

        [Fact]
        public void TestPreprocessorTokenVaue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', TokenKind.Comma).Add('$', new PreprocessorTokenValue());
            TestPattern(patterns, "a{${//}a", PatternA, TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, "a{${}a", PatternA, TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, "{${}a", TokenKind.Preprocessor, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "a{$", PatternA, TokenKind.Preprocessor);
        }


        [Fact]
        public void TestControlCharTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new ControlCharacterClass(), new ControlTokenGroupValue());
            TestPattern(patterns, "");
            TestPattern(patterns, "a\u0000\u0001\u0002\u0003\u0004", PatternA, TokenKind.ControlChar);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "\r", TokenKind.Undefined);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "\n", TokenKind.Undefined);
        }

        [Fact]
        public void TestWhitespaceCharTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new WhiteSpaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            TestPattern(patterns, "");
            TestPattern(patterns, "a    a", PatternA, TokenKind.WhiteSpace, PatternA);
            TestPattern(patterns, "   ", TokenKind.WhiteSpace);
            TestPattern(patterns, "\t\r\n\r\f", TokenKind.WhiteSpace);
            TestPattern(patterns, "aa\na", PatternA, PatternA, TokenKind.WhiteSpace, PatternA);
        }

        [Fact]
        public void TestDigitTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new NumberCharacterClass(), new DigitTokenGroupValue());
            TestPattern(patterns, "1", TokenKind.Integer);
            TestPattern(patterns, "1234567890", TokenKind.Integer);
            TestPattern(patterns, "000", TokenKind.Integer);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "３", TokenKind.Undefined);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "1３", TokenKind.Integer, TokenKind.Undefined);
        }

        [Fact]
        public void TestHexNumberTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('$', new HexNumberTokenValue());
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "$", TokenKind.HexNumber);
            TestPattern(patterns, "$1234567890", TokenKind.HexNumber);
            TestPattern(patterns, "$ABCDEF", TokenKind.HexNumber);
            TestPattern(patterns, "$abcdef", TokenKind.HexNumber);
            TestPattern(patterns, "$000000", TokenKind.HexNumber);
            TestPattern(patterns, "$1234FFFF", TokenKind.HexNumber);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "$CEFO", TokenKind.HexNumber, TokenKind.Undefined);
            Assert.AreEqual(0x123F, HexNumberTokenValue.Unwrap(TestPattern(patterns, "$123F", TokenKind.HexNumber)));
        }

        [Fact]
        public void TestIdentifierTokenValue() {
            var patterns = new InputPatterns();
            var tokens = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["a"] = PatternA
            };
            var tgv = new IdentifierTokenGroupValue(tokens);
            patterns.AddPattern('b', PatternB);
            patterns.AddPattern(new IdentifierCharacterClass(), tgv);
            patterns.AddPattern(new WhiteSpaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            tgv.AllowAmpersand = true;
            tgv.AllowDigits = false;
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "&a", TokenKind.Identifier);
            TestPattern(patterns, "_a", TokenKind.Identifier);
            TestPattern(patterns, "€€__画像", TokenKind.Identifier);
            TestPattern(patterns, "a b caaa", PatternA, TokenKind.WhiteSpace, PatternB, TokenKind.WhiteSpace, TokenKind.Identifier);
            tgv.AllowAmpersand = false;
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "&a", TokenKind.Undefined, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "a9", PatternA, TokenKind.Undefined);
            tgv.AllowDigits = true;
            TestPattern(patterns, "a9", TokenKind.Identifier);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "a.a9", PatternA, TokenKind.Undefined, TokenKind.Identifier);
            TestPattern(patterns, "_a_aaA123_33", TokenKind.Identifier);
            tgv.AllowDots = true;
            TestPattern(patterns, "a.9", TokenKind.Identifier);
        }

        [Fact]
        public void TestEndOfLineCommentTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('/', TokenKind.Slash).Add('/', new EndOfLineCommentTokenGroupValue());
            patterns.AddPattern(new WhiteSpaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            TestPattern(patterns, "/", TokenKind.Slash);
            TestPattern(patterns, "//", TokenKind.Comment);
            TestPattern(patterns, "// / / /", TokenKind.Comment);
            TestPattern(patterns, "/// / / /", TokenKind.Comment);
            TestPattern(patterns, "/ // / / /", TokenKind.Slash, TokenKind.WhiteSpace, TokenKind.Comment);
            TestPattern(patterns, "/ // / /\n /", TokenKind.Slash, TokenKind.WhiteSpace, TokenKind.Comment, TokenKind.WhiteSpace, TokenKind.Slash);
        }

        [Fact]
        public void TestNumberTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern(new NumberCharacterClass(), new NumberTokenGroupValue());
            patterns.AddPattern('.', TokenKind.Dot);
            TestPattern(patterns, "9", TokenKind.Integer);
            TestPattern(patterns, "9.9", TokenKind.Real);
            TestPattern(patterns, "9999.9999", TokenKind.Real);
            TestPattern(patterns, "9.", TokenKind.Integer, TokenKind.Dot);
            TestPattern(patterns, "9.9.", TokenKind.Real, TokenKind.Dot);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e", TokenKind.Real);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e+", TokenKind.Real);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e-", TokenKind.Real);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e.", TokenKind.Real, TokenKind.Dot);
            TestPattern(patterns, "9999.9999E+3", TokenKind.Real);
            TestPattern(patterns, "9999.9999E-3", TokenKind.Real);
            TestPattern(patterns, "9999.9999E-3.", TokenKind.Real, TokenKind.Dot);
        }

        [Fact]
        public void TestDoubleQuotedStringTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('"', new DoubleQuoteStringGroupTokenValue());
            TestPattern(patterns, "\"aaaaaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"aaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"\"\"aaa\"", TokenKind.DoubleQuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "\"aaaaaa", TokenKind.DoubleQuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "\"", TokenKind.DoubleQuotedString);
        }

        [Fact]
        public void TestQuotedStringTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('\'', new QuotedStringTokenValue());
            TestPattern(patterns, "'aaaaaa'", TokenKind.QuotedString);
            TestPattern(patterns, "'aaa''aaa'", TokenKind.QuotedString);
            TestPattern(patterns, "'aaa''''aaa'", TokenKind.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "'aaaaaa", TokenKind.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "'", TokenKind.QuotedString);
            Assert.AreEqual("a", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a'", TokenKind.QuotedString)));
            Assert.AreEqual("a'", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a'''", TokenKind.QuotedString)));
            Assert.AreEqual("a'aa", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a''aa'", TokenKind.QuotedString)));
        }

        [Fact]
        public void TestStringGroupTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('.', TokenKind.Dot);
            patterns.AddPattern('#', new StringGroupTokenValue());
            patterns.AddPattern('\'', new StringGroupTokenValue());
            Assert.AreEqual("a\nb", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a'#$A'b'", TokenKind.QuotedString)));
            TestPattern(patterns, "'aaa'", TokenKind.QuotedString);
            TestPattern(patterns, "#09", TokenKind.QuotedString);
            TestPattern(patterns, "#09'aaaa'", TokenKind.QuotedString);
            TestPattern(patterns, "#09'aaaa'#09", TokenKind.QuotedString);
            TestPattern(patterns, "#$09'aaaa'#09", TokenKind.QuotedString);
            TestPattern(patterns, "#$09'aaaa'#$09", TokenKind.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "#", TokenKind.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "#$", TokenKind.QuotedString);
            TestPattern(patterns, "'aaaa'#$09", TokenKind.QuotedString);
        }

    }
}
