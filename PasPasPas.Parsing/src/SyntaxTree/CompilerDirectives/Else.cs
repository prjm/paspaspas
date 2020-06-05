#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     syntax part for else conditions
    /// </summary>
    public class ElseDirective : CompilerDirectiveBase {
        private readonly Terminal symbol;

        /// <summary>
        ///     else directive
        /// </summary>
        /// <param name="elseCd"></param>
        public ElseDirective(Terminal elseCd)
            => symbol = elseCd;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            visitor.EndVisit(this);
        }


    }
}
