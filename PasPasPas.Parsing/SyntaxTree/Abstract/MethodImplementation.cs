using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation
    /// </summary>
    public class MethodImplementation : DeclaredSymbol, IDeclaredSymbolTarget, IBlockTarget, IExpression {

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
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                yield return Symbols;
                if (Block != null)
                    yield return Block;
            }
        }

        /// <summary>
        ///     statements
        /// </summary>
        public StatementBase Block { get; set; }

    }
}

