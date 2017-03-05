using System;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     assembler statement
    /// </summary>
    public class AssemblerStatement : AbstractSyntaxPart {

        /// <summary>
        ///     kind of the assembler statement
        /// </summary>
        public AssemblerStatementKind Kind { get; set; }

    }
}
