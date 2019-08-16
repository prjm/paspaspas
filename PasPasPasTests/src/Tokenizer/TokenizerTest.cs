using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Infrastructure.Log;
using PasPasPasTests.Common;
using SharpFloat.FloatingPoint;

namespace PasPasPasTests.Tokenizer {

    public class TokenizerTest : CommonTest {

        public static void TokenizerMessageIsGenerated(uint messageNumber, string input) {
            var messages = new List<ILogMessage>();
            var result = RunTokenizer(input, messages);
            var hasMessage = messages.Any(t => t.MessageID == messageNumber);
            Assert.IsTrue(hasMessage);
        }

        public static Token GetToken(int tokenIndex, string input) {
            var messages = new List<ILogMessage>();
            var result = RunTokenizer(input, messages);

            if (tokenIndex < 0 || tokenIndex >= result.Count)
                return Token.Empty;
            else
                return result[tokenIndex];
        }

        public static IList<Token> RunTokenizer(string input, IList<ILogMessage> messages = null) {
            var result = new List<Token>();
            var messageHandler = new ListLogTarget();
            var path = new FileReference("test.pas");
            var data = CommonApi.CreateResolverForSingleString(path, input);
            var options = Factory.CreateOptions(data, CreateEnvironment());
            var api = Factory.CreateTokenizerApi(options);

            using (var tokenizer = api.CreateTokenizer(data, path)) {
                api.Environment.Log.RegisterTarget(messageHandler);

                while (!tokenizer.AtEof) {
                    tokenizer.FetchNextToken();
                    result.Add(tokenizer.CurrentToken);
                }
            }

            if (messages != null)
                foreach (var message in messageHandler.Messages)
                    messages.Add(message);

            Assert.AreEqual(input, result.Select(t => t.Value).Aggregate((a, b) => a + b));

            return result;
        }


        internal static void IsQuotedString(string input, string value)
            => IsToken(TokenKind.QuotedString, input, input, GetUnicodeStringValue(value));

        internal static void IsWideChar(string input, char value)
            => IsToken(TokenKind.QuotedString, input, input, GetWideCharValue(value));

        internal static void IsInteger(string input, ulong value)
            => IsToken(TokenKind.IntegralNumber, input, input, GetIntegerValue(value));

        internal static void IsInteger(string input, object value)
            => IsToken(TokenKind.IntegralNumber, input, input, value);

        internal static void IsWhitespace(string input)
            => IsToken(TokenKind.WhiteSpace, input, input);

        internal static void IsReal(string input, string value, params Tuple<int, string>[] tokens) {
            Assert.IsTrue(ExtF80.TryParse(value, out var v));
            IsToken(TokenKind.RealNumber, input, input, GetExtendedValue(v), tokens);
        }

        internal static void IsHexNumber(string input, object value)
            => IsToken(TokenKind.HexNumber, input, input, value);

        internal static void IsPreprocessor(string input, string value)
            => IsToken(TokenKind.Preprocessor, input, input, GetUnicodeStringValue(value));

        internal static void IsComment(string input)
            => IsToken(TokenKind.Comment, input, input);

        internal static void IsControlChar(string input)
            => IsToken(TokenKind.ControlChar, input, input);

        internal static void IsAssembler(string input)
            => IsToken(TokenKind.Asm, input, input);

        internal static void IsToken(int tokenKind, string input)
            => IsToken(tokenKind, input, input);

        internal static void IsIdentifier(string input, string output = null) {
            if (output == null)
                output = input;

            IsToken(TokenKind.Identifier, output, input);
        }

        internal static void IsToken(int tokenKind, string tokenValue, string input, params Tuple<int, string>[] tokens)
            => IsToken(tokenKind, tokenValue, input, null, tokens);

        internal static void IsToken(int tokenKind, string tokenValue, string input, object value, params Tuple<int, string>[] tokens) {
            var result = RunTokenizer(input);
            Assert.IsNotNull(result);

            if (tokens.Length < 1) {

                Assert.AreEqual(value, result[0].ParsedValue);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(tokenKind, result[0].Kind);
                Assert.AreEqual(tokenValue, result[0].Value);
            }
            else {
                Assert.AreEqual(tokens.Length, result.Count);
                for (var index = 0; index < tokens.Length; index++) {
                    Assert.AreEqual(tokens[index].Item1, result[index].Kind);
                    Assert.AreEqual(tokens[index].Item2, result[index].Value);
                }
            }
        }


    }





}
