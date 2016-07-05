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

        private readonly Punctuators puncts;

        public TestTokenizer(ParserServices environment, StackedFileReader input)
            : base(environment, input) {
            puncts = new Punctuators();
            puncts.AddPunctuator(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
        }

        protected override Punctuators CharacterClasses
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

    }
}
