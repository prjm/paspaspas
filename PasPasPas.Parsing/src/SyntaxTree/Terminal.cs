﻿using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using static PasPasPas.Parsing.Tokenizer.TokenizerWithLookahead;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     syntax tree terminal
    /// </summary>
    public class Terminal : StandardSyntaxTreeBase {

        private readonly Token token;
        private readonly string prefix;
        private readonly string suffix;

        /// <summary>
        ///     create a new terminal token
        /// </summary>
        /// <param name="baseToken"></param>
        public Terminal(TokenSequence baseToken) {
            token = baseToken.Value;
            prefix = baseToken.Prefix;
            suffix = baseToken.Suffix;
        }

        /// <summary>
        ///     token
        /// </summary>
        public Token Token
            => token;

        /// <summary>
        ///     token value
        /// </summary>
        public string Value
            => Token.Value;

        /// <summary>
        ///     token kind
        /// </summary>
        public int Kind
            => Token.Kind;

        /// <summary>
        ///     suggif
        /// </summary>
        public string Suffix
            => suffix;

        /// <summary>
        ///     prefix
        /// </summary>
        public string Prefix
            => prefix;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}