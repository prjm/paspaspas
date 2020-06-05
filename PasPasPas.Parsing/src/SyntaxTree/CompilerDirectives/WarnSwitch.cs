#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     warn switch
    /// </summary>
    public class WarnSwitch : CompilerDirectiveBase {

        /// <summary>
        ///     create a new warning switch
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="id"></param>
        /// <param name="modeSymbol"></param>
        /// <param name="warningType"></param>
        /// <param name="parsedMode"></param>
        public WarnSwitch(Terminal symbol, Terminal id, Terminal modeSymbol, string warningType, WarningMode parsedMode) {
            Symbol = symbol;
            Id = id;
            ModeSymbol = modeSymbol;
            WarningType = warningType;
            Mode = parsedMode;
        }

        /// <summary>
        ///     warning mode
        /// </summary>
        public WarningMode Mode { get; }

        /// <summary>
        ///     mode symbol
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     warning type
        /// </summary>
        public string WarningType { get; }

        /// <summary>
        ///        warn symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     warning id
        /// </summary>
        public Terminal Id { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Id, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            visitor.EndVisit(this);
        }
    }
}
