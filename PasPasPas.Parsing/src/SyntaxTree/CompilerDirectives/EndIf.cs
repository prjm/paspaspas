#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     end of conditional compilation
    /// </summary>
    public class EndIf : CompilerDirectiveBase {
        private readonly Terminal symbol;

        /// <summary>
        ///     end if symbol
        /// </summary>
        /// <param name="terminal"></param>
        public EndIf(Terminal terminal)
            => symbol = terminal;

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
