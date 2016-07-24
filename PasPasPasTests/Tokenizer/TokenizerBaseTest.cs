using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Parser;
using PasPasPas.Api;
using PasPasPas.Infrastructure.Log;

namespace PasPasPasTests.Tokenizer {


    internal class TestTokenizer : TokenizerBase {

        private readonly InputPatterns puncts;

        public TestTokenizer(ParserServices environment, StackedFileReader input)
            : base(environment, input) {
            puncts = new InputPatterns();
            puncts.AddPattern(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
        }

        protected override InputPatterns CharacterClasses
            => puncts;
    }

    [TestClass]
    public class TokenizerBaseTest {

        private const string TestFileName = "test_file_name.pas";

        protected IList<PascalToken> RunTestTokenizer(string input) {
            var result = new List<PascalToken>();
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


        [TestMethod]
        public void SimpleTests() {
            Assert.AreEqual(0, RunTestTokenizer(string.Empty).Count);
            Assert.AreEqual(1, RunTestTokenizer(" \n\n  ").Count);
            Assert.AreEqual(3, RunTestTokenizer(" \n\n  ")[0].EndPosition.Line);
        }

        [TestMethod]
        public void TestSimpleCharClass() {
            SingleCharClass cc = new SingleCharClass('x');
            Assert.IsTrue(cc.Matches('x'));
            Assert.IsFalse(cc.Matches('y'));
            Assert.IsFalse(cc.Matches('\0'));
        }

        [TestMethod]
        public void TestControlCharClass() {
            ControlCharacterClass cc = new ControlCharacterClass();
            Assert.IsTrue(cc.Matches('\a'));
            Assert.IsFalse(cc.Matches('\r'));
            Assert.IsFalse(cc.Matches('\n'));
        }

        [TestMethod]
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

        [TestMethod]
        public void TestExponentCharClass() {
            var cc = new ExponentCharacterClass();
            Assert.IsTrue(cc.Matches('E'));
            Assert.IsTrue(cc.Matches('e'));
            Assert.IsFalse(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        [TestMethod]
        public void TestPlusMinusCharClass() {
            var cc = new PlusMinusCharacterClass();
            Assert.IsTrue(cc.Matches('+'));
            Assert.IsTrue(cc.Matches('-'));
            Assert.IsFalse(cc.Matches('A'));
            Assert.IsFalse(cc.Matches(' '));
        }

        [TestMethod]
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

        private IList<PascalToken> RunTestPattern(InputPatterns patterns, Guid expectedMessage, string input) {
            var result = new List<PascalToken>();
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

        public PascalToken TestPattern(InputPatterns patterns, string input, params int[] tokenValues)
            => TestPattern(patterns, Guid.Empty, input, tokenValues);

        public PascalToken TestPattern(InputPatterns patterns, Guid expectedMessage, string input, params int[] tokenValues) {
            IList<PascalToken> result = RunTestPattern(patterns, expectedMessage, input);
            Assert.AreEqual(tokenValues.Length, result.Count);
            for (int i = 0; i < result.Count; i++)
                Assert.AreEqual(tokenValues[i], result[i].Kind);

            if (result.Count > 0)
                return result[0];

            return null;
        }

        [TestMethod]
        public void TestSimpleInputPatterns() {
            var patterns = new InputPatterns();
            TestPattern(patterns, "");
            TestPattern(patterns, "xx", PascalToken.Undefined, PascalToken.Undefined);
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('b', PatternB);
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "aa", PatternA, PatternA);
            TestPattern(patterns, "b", PatternB);
        }

        [TestMethod]
        public void TestCurlyBraceCommentTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', new CurlyBraceCommentTokenValue());
            TestPattern(patterns, "a{}", PatternA, PascalToken.Comment);
            TestPattern(patterns, "{}", PascalToken.Comment);
            TestPattern(patterns, "{}a", PascalToken.Comment, PatternA);
            TestPattern(patterns, "a{}a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a{//}a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a{(**)}a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a{{}a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "a{", PatternA, PascalToken.Comment);
        }


        [TestMethod]
        public void TestAlternativeCurlyBraceCommentTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('(', PascalToken.OpenParen).Add('*', new AlternativeCurlyBraceCommenTokenValue());
            TestPattern(patterns, "a(", PatternA, PascalToken.OpenParen);
            TestPattern(patterns, "(**)", PascalToken.Comment);
            TestPattern(patterns, "(*a*)", PascalToken.Comment);
            TestPattern(patterns, "(*(*(*(*a*)", PascalToken.Comment);
            TestPattern(patterns, "a(*a*)a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "(**)a", PascalToken.Comment, PatternA);
            TestPattern(patterns, "a(**)a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a(*\n\n*)a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a(*\n***())()()\n*)a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a(*{//}*)a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, "a(*(**)a", PatternA, PascalToken.Comment, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "a(*", PatternA, PascalToken.Comment);
        }

        [TestMethod]
        public void TestPreprocessorTokenVaue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('{', PascalToken.Comma).Add('$', new PreprocessorTokenValue());
            TestPattern(patterns, "a{${//}a", PatternA, PascalToken.Preprocessor, PatternA);
            TestPattern(patterns, "a{${}a", PatternA, PascalToken.Preprocessor, PatternA);
            TestPattern(patterns, "{${}a", PascalToken.Preprocessor, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "a{$", PatternA, PascalToken.Preprocessor);
        }


        [TestMethod]
        public void TestControlCharTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new ControlCharacterClass(), new ControlTokenGroupValue());
            TestPattern(patterns, "");
            TestPattern(patterns, "a\u0000\u0001\u0002\u0003\u0004", PatternA, PascalToken.ControlChar);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "\r", PascalToken.Undefined);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "\n", PascalToken.Undefined);
        }

        [TestMethod]
        public void TestWhitespaceCharTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            TestPattern(patterns, "");
            TestPattern(patterns, "a    a", PatternA, PascalToken.WhiteSpace, PatternA);
            TestPattern(patterns, "   ", PascalToken.WhiteSpace);
            TestPattern(patterns, "\t\r\n\r\f", PascalToken.WhiteSpace);
            TestPattern(patterns, "aa\na", PatternA, PatternA, PascalToken.WhiteSpace, PatternA);
        }

        [TestMethod]
        public void TestDigitTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern(new NumberCharacterClass(), new DigitTokenGroupValue());
            TestPattern(patterns, "1", PascalToken.Integer);
            TestPattern(patterns, "1234567890", PascalToken.Integer);
            TestPattern(patterns, "000", PascalToken.Integer);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "３", PascalToken.Undefined);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "1３", PascalToken.Integer, PascalToken.Undefined);
        }

        [TestMethod]
        public void TestHexNumberTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('a', PatternA);
            patterns.AddPattern('$', new HexNumberTokenValue());
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "$", PascalToken.HexNumber);
            TestPattern(patterns, "$1234567890", PascalToken.HexNumber);
            TestPattern(patterns, "$ABCDEF", PascalToken.HexNumber);
            TestPattern(patterns, "$abcdef", PascalToken.HexNumber);
            TestPattern(patterns, "$000000", PascalToken.HexNumber);
            TestPattern(patterns, "$1234FFFF", PascalToken.HexNumber);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "$CEFO", PascalToken.HexNumber, PascalToken.Undefined);
        }

        [TestMethod]
        public void TestIdentifierTokenValue() {
            var patterns = new InputPatterns();
            var tokens = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["a"] = PatternA
            };
            var tgv = new IdentifierTokenGroupValue(tokens);
            patterns.AddPattern('b', PatternB);
            patterns.AddPattern(new IdentifierCharacterClass(), tgv);
            patterns.AddPattern(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            tgv.AllowAmpersand = true;
            tgv.AllowDigits = false;
            TestPattern(patterns, "a", PatternA);
            TestPattern(patterns, "&a", PascalToken.Identifier);
            TestPattern(patterns, "_a", PascalToken.Identifier);
            TestPattern(patterns, "€€__画像", PascalToken.Identifier);
            TestPattern(patterns, "_a_aaA123_33", PascalToken.Identifier);
            TestPattern(patterns, "a b caaa", PatternA, PascalToken.WhiteSpace, PatternB, PascalToken.WhiteSpace, PascalToken.Identifier);
            tgv.AllowAmpersand = false;
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "&a", PascalToken.Undefined, PatternA);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "a9", PatternA, PascalToken.Undefined);
            TestPattern(patterns, TokenizerBase.UnexpectedCharacter, "a.9", PatternA, PascalToken.Undefined);
            tgv.AllowDigits = true;
            TestPattern(patterns, "a9", PascalToken.Identifier);
            tgv.AllowDots = true;
            TestPattern(patterns, "a.9", PascalToken.Identifier);
        }

        [TestMethod]
        public void TestEndOfLineCommentTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('/', PascalToken.Slash).Add('/', new EndOfLineCommentTokenGroupValue());
            patterns.AddPattern(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            TestPattern(patterns, "/", PascalToken.Slash);
            TestPattern(patterns, "//", PascalToken.Comment);
            TestPattern(patterns, "// / / /", PascalToken.Comment);
            TestPattern(patterns, "/// / / /", PascalToken.Comment);
            TestPattern(patterns, "/ // / / /", PascalToken.Slash, PascalToken.WhiteSpace, PascalToken.Comment);
            TestPattern(patterns, "/ // / /\n /", PascalToken.Slash, PascalToken.WhiteSpace, PascalToken.Comment, PascalToken.WhiteSpace, PascalToken.Slash);
        }

        [TestMethod]
        public void TestNumberTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern(new NumberCharacterClass(), new NumberTokenGroupValue());
            patterns.AddPattern('.', PascalToken.Dot);
            TestPattern(patterns, "9", PascalToken.Integer);
            TestPattern(patterns, "9.9", PascalToken.Real);
            TestPattern(patterns, "9999.9999", PascalToken.Real);
            TestPattern(patterns, "9.", PascalToken.Integer, PascalToken.Dot);
            TestPattern(patterns, "9.9.", PascalToken.Real, PascalToken.Dot);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e", PascalToken.Real);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e+", PascalToken.Real);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e-", PascalToken.Real);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "9.9e.", PascalToken.Real, PascalToken.Dot);
            TestPattern(patterns, "9999.9999E+3", PascalToken.Real);
            TestPattern(patterns, "9999.9999E-3", PascalToken.Real);
            TestPattern(patterns, "9999.9999E-3.", PascalToken.Real, PascalToken.Dot);
        }

        [TestMethod]
        public void TestDoubleQuotedStringTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('.', PascalToken.Dot);
            patterns.AddPattern('"', new DoubleQuoteStringGroupTokenValue());
            TestPattern(patterns, "\"aaaaaa\"", PascalToken.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"aaa\"", PascalToken.DoubleQuotedString);
            TestPattern(patterns, "\"aaa\"\"\"\"aaa\"", PascalToken.DoubleQuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "\"aaaaaa", PascalToken.DoubleQuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "\"", PascalToken.DoubleQuotedString);
        }

        [TestMethod]
        public void TestQuotedStringTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('.', PascalToken.Dot);
            patterns.AddPattern('\'', new QuotedStringTokenValue());
            TestPattern(patterns, "'aaaaaa'", PascalToken.QuotedString);
            TestPattern(patterns, "'aaa''aaa'", PascalToken.QuotedString);
            TestPattern(patterns, "'aaa''''aaa'", PascalToken.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "'aaaaaa", PascalToken.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "'", PascalToken.QuotedString);
            Assert.AreEqual("a", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a'", PascalToken.QuotedString)));
            Assert.AreEqual("a'", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a'''", PascalToken.QuotedString)));
            Assert.AreEqual("a'aa", QuotedStringTokenValue.Unwrap(TestPattern(patterns, "'a''aa'", PascalToken.QuotedString)));
        }

        [TestMethod]
        public void TestStringGroupTokenValue() {
            var patterns = new InputPatterns();
            patterns.AddPattern('.', PascalToken.Dot);
            patterns.AddPattern('#', new StringGroupTokenValue());
            patterns.AddPattern('\'', new StringGroupTokenValue());
            TestPattern(patterns, "'aaa'", PascalToken.QuotedString);
            TestPattern(patterns, "#09", PascalToken.QuotedString);
            TestPattern(patterns, "#09'aaaa'", PascalToken.QuotedString);
            TestPattern(patterns, "#09'aaaa'#09", PascalToken.QuotedString);
            TestPattern(patterns, "#$09'aaaa'#09", PascalToken.QuotedString);
            TestPattern(patterns, "#$09'aaaa'#$09", PascalToken.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "#", PascalToken.QuotedString);
            TestPattern(patterns, TokenizerBase.UnexpectedEndOfToken, "#$", PascalToken.QuotedString);
            TestPattern(patterns, "'aaaa'#$09", PascalToken.QuotedString);
        }

    }
}
