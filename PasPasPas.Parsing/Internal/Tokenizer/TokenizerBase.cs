﻿using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public abstract class TokenizerBase : MessageGenerator {

        /// <summary>
        ///     dummy constructor
        /// </summary>
        protected TokenizerBase() { }


        private IParserInput input;

        /// <summary>
        ///     parser parser input
        /// </summary>
        public IParserInput Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        /// <summary>
        ///     check if tokens are availiable
        /// </summary>
        /// <returns><c>true</c> if tokens are avaliable</returns>
        public bool HasNextToken()
            => !Input.AtEof;

        /// <summary>
        ///     generates an undefined token
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected PascalToken GenerateUndefinedToken(char c) {
            var value = new string(c, 1);
            LogError(MessageData.UndefinedInputToken, value);
            return new PascalToken() {
                Value = value,
                Kind = PascalToken.Undefined
            };
        }

        /// <summary>
        ///     generates an eof token
        /// </summary>
        /// <returns></returns>
        protected static PascalToken GenerateEofToken()
            => new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty };


        /// <summary>
        ///     used char classes
        /// </summary>
        protected abstract Punctuators CharacterClasses { get; }


        /// <summary>
        ///     fetch the next token
        /// </summary>
        /// <returns>next token</returns>
        public PascalToken FetchNextToken() {
            if (Input.AtEof) {
                return GenerateEofToken();
            }

            char c = Input.NextChar();
            PunctuatorGroup tokenGroup;

            if (CharacterClasses.Match(c, out tokenGroup)) {
                return Punctuators.FetchTokenByGroup(Input, c, tokenGroup);
            }

            return GenerateUndefinedToken(c);
        }
    }
}