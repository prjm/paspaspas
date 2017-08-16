﻿using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using System.Text;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     token definition
    /// </summary>
    public struct Token : IEquatable<Token> {

        /// <summary>
        ///     empty token
        /// </summary>
        public static readonly Token Empty
            = new Token();

        /// <summary>
        ///     empty token
        /// </summary>
        public static readonly Token Eof
            = new Token(TokenKind.Eof, -1, string.Empty);


        /// <summary>
        ///     Token value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Token kind
        /// </summary>
        public int Kind { get; }

        /// <summary>
        ///     token position
        /// </summary>
        public int Position { get; }

        /// <summary>
        ///     invalid tokens before this token
        /// </summary>
        public IEnumerable<Token> InvalidTokensBefore {
            get {
                if (invalidTokensBefore != null) {
                    foreach (var token in invalidTokensBefore)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     invalid tokens after this token
        /// </summary>
        public IEnumerable<Token> InvalidTokensAfter {
            get {
                if (invalidTokensAfter != null) {
                    foreach (var token in invalidTokensAfter)
                        yield return token;
                }
            }
        }

        public object ParsedValue { get; set; }

        /// <summary>
        ///     list of invalid tokens before this token
        /// </summary>
        private IList<Token> invalidTokensBefore;

        /// <summary>
        ///     list of invalid tokens after this token
        /// </summary>
        private IList<Token> invalidTokensAfter;

        /// <summary>
        ///     create a new syntax token
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="tokenPosition">token position</param>
        /// <param name="value">token value</param>
        public Token(int tokenKind, int tokenPosition, string value) : this() {
            Kind = tokenKind;
            Position = tokenPosition;
            Value = value.Pool();
        }

        /// <summary>
        ///     create a new syntax token
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="tokenPosition">token position</param>
        /// <param name="value">token value</param>
        public Token(int tokenKind, int tokenPosition, char value) : this() {
            Kind = tokenKind;
            Position = tokenPosition;
            Value = value.Pool();
        }

        public Token(int tokenKind, StringBuilder buffer) : this() {
            Kind = tokenKind;
            Position = -1;
            Value = buffer.ToString().Pool();
        }

        public Token(int tokenId, ITokenizerState state) : this() {
            Kind = tokenId;
            Position = -1;
            Value = state.GetBufferContent().Pool();
        }


        /// <summary>
        ///     assign remaining tokens
        /// </summary>
        /// <param name="invalidTokens"></param>
        /// <param name="afterwards">add tokens after this token</param>
        public void AssignInvalidTokens(IndexedQueue<Token> invalidTokens, bool afterwards) {

            if (invalidTokens.Count > 0) {

                if (afterwards && invalidTokensAfter == null)
                    invalidTokensAfter = new List<Token>(invalidTokens.Count);
                else if (invalidTokensBefore == null)
                    invalidTokensBefore = new List<Token>(invalidTokens.Count);


                foreach (var token in invalidTokens)
                    if (afterwards)
                        invalidTokensAfter.Add(token);
                    else
                        invalidTokensBefore.Add(token);

                invalidTokens.Clear();
            }
        }

        /// <summary>
        ///     format token as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{Kind}: {Value} [{Position}]";

        /// <summary>
        ///     compare to another token
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Token other)
            => (Kind == other.Kind) && (string.Equals(Value, other.Value));
    }

}

