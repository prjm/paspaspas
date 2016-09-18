using PasPasPas.Parsing.Parser;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     syntax tree terminal
    /// </summary>
    public class Terminal : SyntaxPartBase {

        private Token token;

        /// <summary>
        ///     create a new terminal token
        /// </summary>
        /// <param name="baseToken"></param>
        public Terminal(Token baseToken) {
            token = baseToken;
        }

        /// <summary>
        ///     token
        /// </summary>
        public Token Token
            => token;

        /// <summary>
        ///     empty part list
        /// </summary>
        public override IList<ISyntaxPart> Parts { get; }
            = EmptyCollection<ISyntaxPart>.Instance;

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
    }
}