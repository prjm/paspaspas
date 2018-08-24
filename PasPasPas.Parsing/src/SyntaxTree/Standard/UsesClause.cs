using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     uses clause
    /// </summary>
    public class UsesClause : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new uses clause
        /// </summary>
        /// <param name="usesSymbol"></param>
        /// <param name="usesList"></param>
        public UsesClause(Terminal usesSymbol, NamespaceNameListSymbol usesList) {
            UsesSymbol = usesSymbol;
            UsesList = usesList;
        }

        /// <summary>
        ///     names of the units to use
        /// </summary>
        public NamespaceNameListSymbol UsesList { get; }

        /// <summary>
        ///     uses symbol
        /// </summary>
        public Terminal UsesSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, UsesSymbol, visitor);
            AcceptPart(this, UsesList, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => UsesSymbol.GetSymbolLength() +
                UsesList.GetSymbolLength();

    }
}