﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     simple token group value: no more characters
    /// </summary>
    public class SimpleTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     creates a new simple token without suffix
        /// </summary>
        /// <param name="completePrefix">complete prefix</param>
        /// <param name="tokenValue"></param>
        public SimpleTokenGroupValue(int tokenValue, string completePrefix) : base() {
            TokenId = tokenValue;
            Prefix = completePrefix;
        }

        /// <summary>
        ///     token kind
        /// </summary>
        public int TokenId { get; set; }

        /// <summary>
        ///     prefix
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        ///     create a simple token
        /// </summary>
        public override Token Tokenize(TokenizerState state)
            => new Token(TokenId, state.GetBufferContent());


    }
}