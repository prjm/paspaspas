using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit head
    /// </summary>
    public class UnitHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new unit head symbol
        /// </summary>
        /// <param name="terminal1"></param>
        /// <param name="namespaceName"></param>
        /// <param name="hintingInformationList"></param>
        /// <param name="terminal2"></param>
        public UnitHeadSymbol(Terminal terminal1, NamespaceNameSymbol namespaceName, HintingInformationListSymbol hintingInformationList, Terminal terminal2) {
            Unit = terminal1;
            UnitName = namespaceName;
            Hint = hintingInformationList;
            Semicolon = terminal2;
        }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public ISyntaxPart Hint { get; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceNameSymbol UnitName { get; }

        /// <summary>
        ///     unit symbol
        /// </summary>
        public Terminal Unit { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Unit.GetSymbolLength() +
                UnitName.GetSymbolLength() +
                Hint.GetSymbolLength() +
                Semicolon.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Unit, visitor);
            AcceptPart(this, UnitName, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}