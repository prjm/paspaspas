using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     design-time only switch
    /// </summary>
    public class DesignOnly : CompilerDirectiveBase {

        /// <summary>
        ///     design time only directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="designTimeOnly"></param>
        public DesignOnly(Terminal symbol, Terminal mode, DesignOnlyUnit designTimeOnly) {
            Symbol = symbol;
            Mode = mode;
            DesignTimeOnly = designTimeOnly;
        }


        /// <summary>
        ///     switch value
        /// </summary>
        public DesignOnlyUnit DesignTimeOnly { get; }

        /// <summary>
        ///     switch mode
        /// </summary>
        public Terminal Mode { get; }

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
            AcceptPart(this, Mode, visitor);
            visitor.EndVisit(this);
        }

    }
}
