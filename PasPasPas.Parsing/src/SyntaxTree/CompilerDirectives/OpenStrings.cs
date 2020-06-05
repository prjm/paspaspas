#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     open strings switch
    /// </summary>
    public class OpenStrings : CompilerDirectiveBase {


        /// <summary>
        ///     create a new open strings directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public OpenStrings(Terminal symbol, Terminal mode, OpenStringTypeMode parsedMode) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     switch mode
        /// </summary>
        public OpenStringTypeMode Mode { get; }

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
