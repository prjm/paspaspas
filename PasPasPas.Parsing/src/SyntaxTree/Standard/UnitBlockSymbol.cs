using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit block
    /// </summary>
    public class UnitBlockSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new unit block symbol
        /// </summary>
        /// <param name="endSymbol"></param>
        public UnitBlockSymbol(Terminal endSymbol)
            => EndSymbol = endSymbol;

        /// <summary>
        ///     create a new unit block symbol
        /// </summary>
        /// <param name="mainBlock"></param>
        public UnitBlockSymbol(CompoundStatementSymbol mainBlock)
            => MainBlock = mainBlock;

        /// <summary>
        ///     create a new unit block symbol
        /// </summary>
        /// <param name="initialization"></param>
        /// <param name="endSymbol"></param>
        public UnitBlockSymbol(UnitInitializationSymbol initialization, Terminal endSymbol) {
            Initialization = initialization;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     initialization
        /// </summary>
        public UnitInitializationSymbol Initialization { get; }

        /// <summary>
        ///     main block
        /// </summary>
        public CompoundStatementSymbol MainBlock { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Initialization.GetSymbolLength() +
                MainBlock.GetSymbolLength() +
                EndSymbol.GetSymbolLength();

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, MainBlock, visitor);
            AcceptPart(this, Initialization, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }


    }
}