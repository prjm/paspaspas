using PasPasPas.Parsing.Parser;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     syntax tree terminal
    /// </summary>
    public class Terminal : SyntaxPartBase {

        private PascalToken pascalToken;

        /// <summary>
        ///     create a new terminal token
        /// </summary>
        /// <param name="pascalToken"></param>
        public Terminal(PascalToken pascalToken) {
            this.pascalToken = pascalToken;
        }

        /// <summary>
        ///     token
        /// </summary>
        public PascalToken Token
            => pascalToken;

        /// <summary>
        ///     empty part list
        /// </summary>
        public override IEnumerable<ISyntaxTreeNode> Parts { get; }
            = EmptyCollection<ISyntaxTreeNode>.Instance;

        /// <summary>
        ///     token value
        /// </summary>
        public string Value
            => Token.Value;
    }
}