using PasPasPas.Globals.Parsing;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     hpp-emit directive
    /// </summary>
    public class HppEmit : CompilerDirectiveBase {

        /// <summary>
        ///     create a new hpp emit directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="modeSymbol"></param>
        /// <param name="modeValue"></param>
        /// <param name="mode"></param>
        public HppEmit(Terminal symbol, Terminal modeSymbol, Terminal modeValue, HppEmitMode mode) {
            Symbol = symbol;
            ModeSymbol = modeSymbol;
            ModeValue = modeValue;
            Mode = mode;
        }

        /// <summary>
        ///     value to emit
        /// </summary>
        public string EmitValue
            => ModeValue?.Value;

        /// <summary>
        ///     emit mode
        /// </summary>
        public HppEmitMode Mode { get; }

        /// <summary>
        ///     hpp emit
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     emit mode
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     emit value
        /// </summary>
        public Terminal ModeValue { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            AcceptPart(this, ModeValue, visitor);
            visitor.EndVisit(this);
        }
    }
}
