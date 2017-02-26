using System;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation
    /// </summary>
    public class MethodImplementation : AbstractSyntaxPart, IDeclaredSymbolTarget, IStatementTarget {

        /// <summary>
        ///     symbols
        /// </summary>
        public DeclaredSymbols Symbols { get; }
            = new DeclaredSymbols();

        /// <summary>
        ///     method kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     statements
        /// </summary>
        public BlockOfStatements Statements { get; }
            = new BlockOfStatements();

    }
}

