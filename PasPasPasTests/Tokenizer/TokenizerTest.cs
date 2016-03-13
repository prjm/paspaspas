using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Internal;

namespace PasPasPasTests.Tokenizer {

    [TestClass]
    public class TokenizerTest {

        [TestMethod]
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

        [TestMethod]
        public void TestIntegers() {
            Assert.IsInteger("123");
            Assert.IsInteger("0000");
            Assert.IsInteger("10000");
        }

        [TestMethod]
        public void TestWhitespace() {
            Assert.IsWhitespace(" ");
            Assert.IsWhitespace(" " + System.Environment.NewLine);
            Assert.IsWhitespace("  ");
        }

        [TestMethod]
        public void TestRealNumbers() {
            Assert.IsReal("123.123");
            Assert.IsReal("123E+10");
            Assert.IsReal("123E-10");
            Assert.IsReal("123.123E-10");
            Assert.IsReal("123.123E+10");
        }

        [TestMethod]
        public void TestIsPreprocessorCommand() {
            Assert.IsPreprocessor("{$A}");
            Assert.IsComment("(*$HPPEMIT '}'*)");
        }

        [TestMethod]
        public void TestComment() {
            Assert.IsComment("// dsfssdf ");
            Assert.IsComment("{ dsfssdf }");
            Assert.IsComment("(* HPPEMIT '}'*)");
        }


        [TestMethod]
        public void TestHexNumbers() {
            Assert.IsHexNumber("$333F");
            Assert.IsHexNumber("$000000");
        }

        [TestMethod]
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

        [TestMethod]
        public void TestMessages() {
            Assert.TokenizerMessageIsGenerated(MessageData.IncompleteHexNumber, "$");
            Assert.TokenizerMessageIsGenerated(MessageData.IncompleteIdentifier, "&");
            Assert.TokenizerMessageIsGenerated(MessageData.UndefinedInputToken, "´");
        }
    }
}
