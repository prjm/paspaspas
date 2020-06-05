#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     <c>$ELSEIF</c> directive
    /// </summary>
    public class ElseIfDirective : CompilerDirectiveBase {
        private readonly Terminal symbol;

        /// <summary>
        ///     else if directive
        /// </summary>
        /// <param name="terminal"></param>
        public ElseIfDirective(Terminal terminal)
            => symbol = terminal;

        /// <summary>
        ///     accept syntax part visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            visitor.EndVisit(this);
        }
    }
}
