#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     hints directive
    /// </summary>
    public class Hints : CompilerDirectiveBase {

        /// <summary>
        ///     create a new hints directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="modeSymbol"></param>
        /// <param name="parsedMode"></param>
        public Hints(Terminal symbol, Terminal modeSymbol, CompilerHint parsedMode) {
            Symbol = symbol;
            ModeSymbol = modeSymbol;
            Mode = parsedMode;
        }

        /// <summary>
        ///     hint mode
        /// </summary>
        public CompilerHint Mode { get; }

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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            visitor.EndVisit(this);
        }
    }
}

