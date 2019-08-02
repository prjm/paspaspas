using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     extended syntax directive
    /// </summary>
    public class ExtSyntax : CompilerDirectiveBase {

        /// <summary>
        ///     create a new extended syntax directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public ExtSyntax(Terminal symbol, Terminal mode, ExtendedSyntax parsedMode) {
            Symbol = symbol;
            Mode1 = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     syntax mode
        /// </summary>
        public ExtendedSyntax Mode { get; }

        /// <summary>
        ///     directive symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     switch mode
        /// </summary>
        public Terminal Mode1 { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Mode1, visitor);
            visitor.EndVisit(this);
        }
    }
}
