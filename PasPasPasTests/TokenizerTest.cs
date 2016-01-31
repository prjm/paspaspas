﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Internal;

namespace PasPasPasTests {

    [TestClass]
    public class TokenizerTest {

        [TestMethod]
        [TestCategory("Tokenizer")]
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
        [TestCategory("Tokenizer")]
        public void TestIntegers() {
            Assert.IsInteger("123");
            Assert.IsInteger("0000");
            Assert.IsInteger("10000");
        }

        [TestMethod]
        [TestCategory("Tokenizer")]
        public void TestRealNumbers() {
            Assert.IsReal("123.123");
            Assert.IsReal("123E+10");
            Assert.IsReal("123E-10");
            Assert.IsReal("123.123E-10");
            Assert.IsReal("123.123E+10");
        }


        [TestMethod]
        [TestCategory("Tokenizer")]
        public void TestHexNumbers() {
            Assert.IsHexNumber("$333F");
            Assert.IsHexNumber("$000000");
        }

        [TestMethod]
        [TestCategory("Tokenizer")]
        public void TestQuotedString() {
            Assert.IsQuotedString("''");
            Assert.IsQuotedString("'sdfddfsd'");
        }

        [TestMethod]
        [TestCategory("Tokenizer")]
        public void TestMessages() {
            Assert.TokenizerMessageIsGenerated(MessageData.IncompleteHexNumber, "$");
            Assert.TokenizerMessageIsGenerated(MessageData.IncompleteIdentifier, "&");
            Assert.TokenizerMessageIsGenerated(MessageData.UndefinedInputToken, "´");
        }
    }
}
