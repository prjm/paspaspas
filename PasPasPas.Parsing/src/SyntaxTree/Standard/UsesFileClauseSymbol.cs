using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     uses clause with file names
    /// </summary>
    public class UsesFileClauseSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     generate a new uses file clause
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="namespaceFileNameList"></param>
        public UsesFileClauseSymbol(Terminal terminal, NamespaceFileNameListSymbol namespaceFileNameList) {
            UsesSymbol = terminal;
            Files = namespaceFileNameList;
        }

        /// <summary>
        ///     Namespace files
        /// </summary>
        public NamespaceFileNameListSymbol Files { get; }

        /// <summary>
        ///     uses symbol
        /// </summary>
        public Terminal UsesSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, UsesSymbol, visitor);
            AcceptPart(this, Files, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => UsesSymbol.GetSymbolLength() +
                Files.GetSymbolLength();

    }
}
