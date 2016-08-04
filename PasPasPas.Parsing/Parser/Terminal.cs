using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Parser {

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
        ///     empty part list
        /// </summary>
        public override ICollection<ISyntaxPart> Parts { get; }
            = EmptyCollection<ISyntaxPart>.Instance;
    }
}