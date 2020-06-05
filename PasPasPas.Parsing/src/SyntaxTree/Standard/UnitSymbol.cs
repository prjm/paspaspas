#nullable disable
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     definition of a unit
    /// </summary>
    public class UnitSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationListSymbol Hints
            => UnitHead?.Hint as HintingInformationListSymbol;

        /// <summary>
        ///     unit block
        /// </summary>
        public UnitBlockSymbol UnitBlock { get; set; }

        /// <summary>
        ///     unit head section
        /// </summary>
        public UnitHeadSymbol UnitHead { get; set; }

        /// <summary>
        ///     unit implementation section
        /// </summary>
        public UnitImplementationSymbol UnitImplementation { get; set; }

        /// <summary>
        ///     unit interface
        /// </summary>
        public UnitInterfaceSymbol UnitInterface { get; set; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceNameSymbol UnitName
            => UnitHead?.UnitName;

        /// <summary>
        ///     path of the unit
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     dot syntax element
        /// </summary>
        public Terminal DotSymbol { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => UnitHead.GetSymbolLength() +
                UnitInterface.GetSymbolLength() +
                UnitImplementation.GetSymbolLength() +
                UnitBlock.GetSymbolLength() +
                DotSymbol.GetSymbolLength();

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, UnitHead, visitor);
            AcceptPart(this, UnitInterface, visitor);
            AcceptPart(this, UnitImplementation, visitor);
            AcceptPart(this, UnitBlock, visitor);
            AcceptPart(this, DotSymbol, visitor);
            visitor.EndVisit(this);
        }

    }
}
