using PasPasPas.Parsing.Tokenizer;
using Xunit;

namespace PasPasPasTests.Tokenizer {

    public class TokenizerTest {

        [Fact]
        public void TestIdentifiers() {
            Assert.IsIdentifier("abcd");
            Assert.IsIdentifier("ABCD");
            Assert.IsIdentifier("öäöäö");
            Assert.IsIdentifier("_dummy");
            Assert.IsIdentifier("du_mmy");
            Assert.IsIdentifier("dummy_");
            Assert.IsIdentifier("&program", "program");
            Assert.IsIdentifier("asd994");
        }

        [Fact]
        public void TestIntegers() {
            Assert.IsInteger("123");
            Assert.IsInteger("0000");
            Assert.IsInteger("10000");
        }

        [Fact]
        public void TestWhitespace() {
            Assert.IsWhitespace(" ");
            Assert.IsWhitespace(" " + System.Environment.NewLine);
            Assert.IsWhitespace("  ");
        }

        [Fact]
        public void TestRealNumbers() {
            Assert.IsReal("123.123");
            Assert.IsReal("123E+10");
            Assert.IsReal("123E-10");
            Assert.IsReal("123.123E-10");
            Assert.IsReal("123.123E+10");
        }

        [Fact]
        public void TestIsPreprocessorCommand() {
            Assert.IsPreprocessor("{$A}");
            Assert.IsComment("(*$HPPEMIT '}'*)");
        }

        [Fact]
        public void TestComment() {
            Assert.IsComment("// dsfssdf ");
            Assert.IsComment("{ dsfssdf }");
            Assert.IsComment("(* HPPEMIT '}'*)");
        }


        [Fact]
        public void TestHexNumbers() {
            Assert.IsHexNumber("$333F");
            Assert.IsHexNumber("$000000");
        }

        [Fact]
        public void TestQuotedString() {
            Assert.IsQuotedString("''");
            Assert.IsQuotedString("'sdfddfsd'");
            Assert.IsQuotedString("'sdf''ddfsd'");
            Assert.IsQuotedString("#45");
            Assert.IsQuotedString("#45'xxx'#55");
            Assert.IsQuotedString("'ddd'#45'xxx'");
            Assert.IsQuotedString("#$F45");
            Assert.IsQuotedString("'ddd'#$1245'xxx'");
        }

        [Fact]
        public void TestMessages() {
            Assert.TokenizerMessageIsGenerated(StandardTokenizer.IncompleteHexNumber, "$");
            Assert.TokenizerMessageIsGenerated(StandardTokenizer.IncompleteIdentifier, "&");
            Assert.TokenizerMessageIsGenerated(TokenizerBase.UnexpectedCharacter, "´");
        }
    }
}
