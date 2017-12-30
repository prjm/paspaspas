using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPasTests.Common;

namespace PasPasPasTests.Tokenizer {

    public class TokenizerTest : CommonTest {

        public void TokenizerMessageIsGenerated(Guid messageNumber, string input) {
            var messages = new List<ILogMessage>();
            var result = RunTokenizer(input, messages);
            var hasMessage = messages.Any(t => t.MessageID == messageNumber);
            Assert.IsTrue(hasMessage);
        }

        public Token GetToken(int tokenIndex, string input) {
            var messages = new List<ILogMessage>();
            var result = RunTokenizer(input, messages);

            if (tokenIndex < 0 || tokenIndex >= result.Count)
                return Token.Empty;
            else
                return result[tokenIndex];
        }

        public IList<Token> RunTokenizer(string input, IList<ILogMessage> messages = null) {
            var result = new List<Token>();
            var messageHandler = new ListLogTarget();
            var api = new TokenizerApi(CreateEnvironment());
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

            Assert.AreEqual(input, result.Select(t => t.Value).Aggregate((a, b) => a + b));

            return result;
        }


        internal void IsQuotedString(string input, string value)
            => IsToken(TokenKind.QuotedString, input, input, GetUnicodeStringValue(value));

        internal void IsQuotedString(string input, char value)
            => IsToken(TokenKind.QuotedString, input, input, GetWideCharValue(value));


        internal void IsInteger(string input, ulong value)
            => IsToken(TokenKind.Integer, input, input, GetIntegerValue(value));

        internal void IsInteger(string input, object value)
            => IsToken(TokenKind.Integer, input, input, value);

        internal void IsWhitespace(string input)
            => IsToken(TokenKind.WhiteSpace, input, input);

        internal void IsReal(string input, double value, params Tuple<int, string>[] tokens)
            => IsToken(TokenKind.Real, input, input, GetExtendedValue(value), tokens);

        internal void IsHexNumber(string input, object value)
            => IsToken(TokenKind.HexNumber, input, input, value);

        internal void IsPreprocessor(string input, string value)
            => IsToken(TokenKind.Preprocessor, input, input, GetUnicodeStringValue(value));

        internal void IsComment(string input)
            => IsToken(TokenKind.Comment, input, input);

        internal void IsControlChar(string input)
            => IsToken(TokenKind.ControlChar, input, input);

        internal void IsAssembler(string input)
            => IsToken(TokenKind.Asm, input, input);

        internal void IsToken(int tokenKind, string input)
            => IsToken(tokenKind, input, input);

        internal void IsIdentifier(string input, string output = null) {
            if (output == null)
                output = input;

            IsToken(TokenKind.Identifier, output, input);
        }

        internal void IsToken(int tokenKind, string tokenValue, string input, params Tuple<int, string>[] tokens)
            => IsToken(tokenKind, tokenValue, input, null, tokens);

        internal void IsToken(int tokenKind, string tokenValue, string input, object value, params Tuple<int, string>[] tokens) {
            var result = RunTokenizer(input);
            Assert.IsNotNull(result);

            if (tokens.Length < 1) {

                if (value is double)
                    Assert.AreEqual((double)value, (result[0].ParsedValue as IRealValue).AsDouble);
                else
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
