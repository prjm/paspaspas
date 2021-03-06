﻿#nullable disable
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for quoted strings
    /// </summary>
    public sealed class QuotedStringTokenValue : PatternContinuation {

        /// <summary>
        ///     quote char
        /// </summary>
        public char QuoteChar { get; }

        /// <summary>
        ///     token id
        /// </summary>
        public int TokenId { get; }

        /// <summary>
        ///     create a new quoted string token value
        /// </summary>
        /// <param name="quoteChar"></param>
        /// <param name="tokenId">token id</param>
        public QuotedStringTokenValue(int tokenId, char quoteChar) {
            QuoteChar = quoteChar;
            TokenId = tokenId;
        }

        /// <summary>
        ///     parse a quoted string
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            var found = false;
            var quote = QuoteChar;
            using (var resultBuilder = state.FetchStringBuilder()) {

                while (!found && !state.AtEof) {
                    var nextChar1 = state.LookAhead(1);
                    var nextChar2 = state.LookAhead(2);

                    found = nextChar1 == quote && nextChar2 != quote;
                    var escapedQuote = nextChar1 == quote && nextChar2 == quote;

                    if (!found)
                        resultBuilder.Item.Append(state.NextChar(true));
                    else
                        state.NextChar(true);

                    if (escapedQuote)
                        state.NextChar(true);
                }

                found = state.BufferEndsWith(QuoteChar) && state.Length > 1;

                if (!found)
                    state.Error(MessageNumbers.IncompleteString);

                IValue value;
                if (resultBuilder.Item.Length == 1)
                    value = state.RuntimeValues.Chars.ToWideCharValue(resultBuilder.Item[0]);
                else
                    value = state.RuntimeValues.Strings.ToUnicodeString(state.Environment.StringPool.PoolString(resultBuilder.Item));

                return new Token(TokenId, state.GetBufferContent(), value);
            }
        }
    }

}