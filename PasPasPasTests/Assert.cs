using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Service;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using A = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace PasPasPasTests {

    public static class TestHelper {

        public static List<PascalToken> RunTokenizer(string input, IList<ILogMessage> messages = null) {
            var environment = new StandardServices();
            var services = new ParserServices(environment);
            var messageHandler = new LogTarget();
            var tokenizer = new StandardTokenizer(services);
            var result = new List<PascalToken>();
            environment.LogManager.RegisterTarget(messageHandler);
            using (var inputString = new StringInput(input, "test.pas"))
            using (var reader = new StackedFileReader()) {
                EventHandler<LogMessageEvent> handler = (_, x) => messages.Add(x.Message);
                reader.AddFile(inputString);
                tokenizer.Input = reader;

                if (messages != null)
                    messageHandler.ProcessMessage += handler;

                while (tokenizer.HasNextToken())
                    result.Add(tokenizer.FetchNextToken());

                return result;
            }
        }
    }

    public static class Assert {

        public static void AreEqual(object expected, object actual) {
            A.AreEqual(expected, actual);
        }

        public static void IsTrue(bool o) {
            A.IsTrue(o);
        }

        public static void IsFalse(bool o) {
            A.IsFalse(o);
        }

        public static void AreNotEqual(object notExpected, object actual) {
            A.AreNotEqual(notExpected, actual);
        }

        public static void IsNotNull(object o) {
            A.IsNotNull(o);
        }

        public static void IsNull(object o) {
            A.IsNull(o);
        }

        public static void IsToken(int tokenKind, string tokenValue, string input) {
            var result = TestHelper.RunTokenizer(input);
            IsNotNull(result);
            AreEqual(1, result.Count);
            AreEqual(tokenKind, result[0].Kind);
            AreEqual(tokenValue, result[0].Value);
        }

        public static void IsIdentifier(string input, string output = null) {
            if (output == null)
                output = input;

            IsToken(PascalToken.Identifier, output, input);
        }

        internal static void TokenizerMessageIsGenerated(Guid messageNumber, string input) {
            var messages = new List<ILogMessage>();
            var result = TestHelper.RunTokenizer(input, messages);
            bool hasMessage = messages.Any(t => t.MessageID == messageNumber);
            IsTrue(hasMessage);
        }

        public static void IsQuotedString(string input) {
            IsToken(PascalToken.QuotedString, input, input);
        }

        public static void IsInteger(string input) {
            IsToken(PascalToken.Integer, input, input);
        }

        public static void IsWhitespace(string input) {
            IsToken(PascalToken.WhiteSpace, input, input);
        }

        public static void IsReal(string input) {
            IsToken(PascalToken.Real, input, input);
        }

        public static void IsHexNumber(string input) {
            IsToken(PascalToken.HexNumber, input, input);
        }

        public static void IsPreprocessor(string input) {
            IsToken(PascalToken.Preprocessor, input, input);
        }

        public static void IsComment(string input) {
            IsToken(PascalToken.Comment, input, input);
        }

    }
}