﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private IList<PascalToken> RunTestPattern(InputPatterns patterns, string input) {
            var result = new List<PascalToken>();
            using (StringInput inputFile = new StringInput(input, new FileReference(TestFileName)))
            using (StackedFileReader reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                while (!reader.AtEof) {
                    result.Add(patterns.FetchNextToken(reader));
                }
                return result;
            };
        }

        public void TestPattern(InputPatterns patterns, string input, params int[] tokenValues) {
            IList<PascalToken> result = RunTestPattern(patterns, input);
            Assert.AreEqual(tokenValues.Length, result.Count);
            for (int i = 0; i < result.Count; i++)
                Assert.AreEqual(tokenValues[i], result[i].Kind);
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

    }
}
