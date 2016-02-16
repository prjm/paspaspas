using PasPasPas.Api;
using PasPasPas.Internal;
using PasPasPas.Internal.Input;
using PasPasPas.Internal.Log;
using PasPasPas.Internal.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using A = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace PasPasPasTests {

    public static class TestHelper {

        public static List<PascalToken> RunTokenizer(string input, IList<LogMessage> messages = null) {
            var tokenizer = new StandardTokenizer();
            var result = new List<PascalToken>();
            EventHandler<LogMessageEventArgs> handler = (_, x) => messages.Add(x.Message);
            tokenizer.Input = new StringInput(input);

            if (messages != null)
                tokenizer.LogMessage += handler;

            while (tokenizer.HasNextToken())
                result.Add(tokenizer.FetchNextToken());

            tokenizer.LogMessage -= handler;

            return result;
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

        internal static void TokenizerMessageIsGenerated(int messageNumber, string input) {
            var messages = new List<LogMessage>();
            var result = TestHelper.RunTokenizer(input, messages);
            bool hasMessage = messages.Any(t => t.Id == messageNumber);
            IsTrue(hasMessage);
        }

        public static void IsQuotedString(string input) {
            IsToken(PascalToken.QuotedString, input.Substring(1, input.Length - 2), input);
        }

        public static void IsInteger(string input) {
            IsToken(PascalToken.Integer, input, input);
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

    }
}