using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     requires clause
    /// </summary>
    public class PackageRequiresSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new requires symbol
        /// </summary>
        /// <param name="requiresSymbol"></param>
        /// <param name="requiresList"></param>
        /// <param name="semicolon"></param>
        public PackageRequiresSymbol(Terminal requiresSymbol, NamespaceNameListSymbol requiresList, Terminal semicolon) {
            RequiresSymbol = requiresSymbol;
            RequiresList = requiresList;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     required packages
        /// </summary>
        public NamespaceNameListSymbol RequiresList { get; }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal RequiresSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, RequiresSymbol, visitor);
            AcceptPart(this, RequiresList, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => RequiresSymbol.GetSymbolLength() +
                RequiresList.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}