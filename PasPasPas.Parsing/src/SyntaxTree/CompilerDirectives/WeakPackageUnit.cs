using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     weak package unit directive
    /// </summary>
    public class WeakPackageUnit : CompilerDirectiveBase {

        /// <summary>
        ///     create a new weak package unit directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public WeakPackageUnit(Terminal symbol, Terminal mode, WeakPackaging parsedMode) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     packaging mode
        /// </summary>
        public WeakPackaging Mode { get; }

        /// <summary>
        ///     mode symbol
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            visitor.EndVisit(this);
        }
    }
}
