#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     old type layout directive
    /// </summary>
    public class OldTypeLayout : CompilerDirectiveBase {

        /// <summary>
        ///     create a new old type layout symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public OldTypeLayout(Terminal symbol, Terminal mode, OldRecordTypeMode parsedMode) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     switch mode
        /// </summary>
        public OldRecordTypeMode Mode { get; }

        /// <summary>
        ///     mode
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            visitor.EndVisit(this);
        }


    }
}