using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     uses clause
    /// </summary>
    public class UsesClauseSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new uses clause
        /// </summary>
        /// <param name="usesSymbol"></param>
        /// <param name="usesList"></param>
        /// <param name="semicolon"></param>
        public UsesClauseSymbol(Terminal usesSymbol, NamespaceNameListSymbol usesList, Terminal semicolon) {
            UsesSymbol = usesSymbol;
            UsesList = usesList;
            Semicolon = semicolon;
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
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => UsesSymbol.GetSymbolLength() +
                UsesList.GetSymbolLength() +
                Semicolon.GetSymbolLength();

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }
    }
}