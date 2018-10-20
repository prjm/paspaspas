using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     debug information switch
    /// </summary>
    public class DebugInfoSwitch : CompilerDirectiveBase {

        /// <summary>
        ///     create a debug info switch directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="debugInfo"></param>
        public DebugInfoSwitch(Terminal symbol, Terminal mode, DebugInformation debugInfo) {
            Symbol = symbol;
            Mode = mode;
            DebugInfo = debugInfo;
        }

        /// <summary>
        ///     debug information mode
        /// </summary>
        public DebugInformation DebugInfo { get; }

        /// <summary>
        ///     mode
        /// </summary>
        public Terminal Mode { get; }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Mode, visitor);
            visitor.EndVisit(this);
        }
    }
}
