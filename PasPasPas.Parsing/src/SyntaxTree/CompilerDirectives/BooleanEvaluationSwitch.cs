using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     boolean evaluation switch
    /// </summary>
    public class BooleanEvaluationSwitch : CompilerDirectiveBase {

        /// <summary>
        ///     create a new boolean evaluation switch
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="evalMode"></param>
        public BooleanEvaluationSwitch(Terminal symbol, Terminal mode, BooleanEvaluation evalMode) {
            Symbol = symbol;
            Mode = mode;
            BoolEval = evalMode;
        }

        /// <summary>
        ///     boolean evaluation mode
        /// </summary>
        public BooleanEvaluation BoolEval { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     mode
        /// </summary>
        public Terminal Mode { get; }

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
