#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     conditional compilation directive (if-def)
    /// </summary>
    public class IfDef : CompilerDirectiveBase {

        /// <summary>
        ///     create a new ifdef symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="negate"></param>
        /// <param name="conditional"></param>
        public IfDef(Terminal symbol, bool negate, Terminal conditional) {
            Symbol = symbol;
            Negate = negate;
            Conditional = conditional;
        }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     inverts the  check for the symbol ("ifndef")
        /// </summary>
        public bool Negate { get; }

        /// <summary>
        ///     conditional
        /// </summary>
        public Terminal Conditional { get; }

        /// <summary>
        ///     symbol to check
        /// </summary>
        public string SymbolName
            => Conditional?.Value;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Conditional, visitor);
            visitor.EndVisit(this);
        }
    }
}
