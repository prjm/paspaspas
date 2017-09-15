using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using System.Collections.Generic;
using A = Xunit.Assert;
using PasPasPas.Api;
using PasPasPas.DesktopPlatform;
using System;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPasTests {

    public static class TestHelper {

        public static IList<Token> RunTokenizer(string input, IList<ILogMessage> messages = null) {
            StaticEnvironment.Clear();

            var result = new List<Token>();
            var messageHandler = new ListLogTarget();
            var options = new TokenizerApiOptions() { KeepWhitespace = true };
            var api = new TokenizerApi(new StandardFileAccess(), options);
            using (var tokenizer = api.CreateTokenizerForString("test.pas", input)) {
                api.Log.RegisterTarget(messageHandler);

                while (!tokenizer.AtEof) {
                    tokenizer.FetchNextToken();
                    result.Add(tokenizer.CurrentToken);
                }
            }

            if (messages != null)
                foreach (var message in messageHandler.Messages)
                    messages.Add(message);

            return result;
        }
    }

    public static class Assert {

        public static void AreEqual(object expected, object actual, string message = "")
            => A.Equal(expected, actual);

        public static void IsTrue(bool o)
            => A.True(o);

        public static void IsFalse(bool o)
            => A.False(o);

        public static void AreNotEqual(object notExpected, object actual)
            => A.NotEqual(notExpected, actual);

        public static void IsNotNull(object o)
            => A.NotNull(o);

        public static void IsNull(object o)
            => A.Null(o);

        public static void Throws<T>(Action testCode)
            => A.Throws(typeof(T), testCode);

    }
}