#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     <c>$IF</c> directive
    /// </summary>
    public class IfDirective : CompilerDirectiveBase {
        private readonly Terminal terminal;

        /// <summary>
        ///     create a new if directive
        /// </summary>
        /// <param name="symbol"></param>
        public IfDirective(Terminal symbol)
            => terminal = symbol;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, terminal, visitor);
            visitor.EndVisit(this);
        }

    }
}
