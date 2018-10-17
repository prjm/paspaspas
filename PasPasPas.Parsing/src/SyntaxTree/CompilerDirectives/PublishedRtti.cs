using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     type info directive
    /// </summary>
    public class PublishedRtti : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal mode;
        private readonly RttiForPublishedProperties parsedMode;

        /// <summary>
        ///     create a new type info directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public PublishedRtti(Terminal symbol, Terminal mode, RttiForPublishedProperties parsedMode) {
            this.symbol = symbol;
            this.mode = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     switch mode
        /// </summary>
        public RttiForPublishedProperties Mode { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, mode, visitor);
            visitor.EndVisit(this);
        }


    }
}
