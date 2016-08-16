using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using A = Xunit.Assert;


namespace PasPasPasTests {

    public static class TestHelper {

        public static List<PascalToken> RunTokenizer(string input, IList<ILogMessage> messages = null) {
            var logManager = new LogManager();
            var services = new ParserServices(logManager);
            var messageHandler = new LogTarget();
            var result = new List<PascalToken>();
            logManager.RegisterTarget(messageHandler);
            using (var inputString = new StringInput(input, new FileReference("test.pas")))
            using (var reader = new StackedFileReader()) {
                EventHandler<LogMessageEvent> handler = (_, x) => messages.Add(x.Message);
                reader.AddFile(inputString);
                var tokenizer = new StandardTokenizer(services, reader);

                if (messages != null)
                    messageHandler.ProcessMessage += handler;

                while (tokenizer.HasNextToken())
                    result.Add(tokenizer.FetchNextToken());

                return result;
            }
        }
    }

    public static class Assert {

        public static void AreEqual(object expected, object actual, string message = "") {
            A.Equal(expected, actual);
        }

        public static void IsTrue(bool o) {
            A.True(o);
        }

        public static void IsFalse(bool o) {
            A.False(o);
        }

        public static void AreNotEqual(object notExpected, object actual) {
            A.NotEqual(notExpected, actual);
        }

        public static void IsNotNull(object o) {
            A.NotNull(o);
        }

        public static void IsNull(object o) {
            A.Null(o);
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
            IsToken(TokenKind.Preprocessor, input, input);
        }

        public static void IsComment(string input) {
            IsToken(TokenKind.Comment, input, input);
        }

    }
}