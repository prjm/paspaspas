using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using System.Collections.Generic;
using A = Xunit.Assert;
using PasPasPas.Api;
using PasPasPas.DesktopPlatform;

namespace PasPasPasTests {

    public static class TestHelper {

        public static IList<Token> RunTokenizer(string input, IList<ILogMessage> messages = null) {

            var result = new List<Token>();
            var messageHandler = new ListLogTarget();
            var api = new TokenizerApi(new StandardFileAccess());
            var tokenizer = api.CreateTokenizerForString("test.pas", input);
            api.Log.RegisterTarget(messageHandler);

            while (!tokenizer.AtEof) {
                tokenizer.FetchNextToken();
                result.Add(tokenizer.CurrentToken);
            }

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

    }
}