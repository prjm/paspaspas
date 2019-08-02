using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     list of program parameters
    /// </summary>
    public class ProgramParameterListSymbol : VariableLengthSyntaxTreeBase<ProgramParameterSymbol> {

        /// <summary>
        ///     create a new parameter list
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="items"></param>
        /// <param name="closeParen"></param>
        public ProgramParameterListSymbol(Terminal openParen, ImmutableArray<ProgramParameterSymbol> items, Terminal closeParen) : base(items) {
            OpenParen = openParen;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenParen.GetSymbolLength() +
                ItemLength +
                CloseParen.GetSymbolLength();
    }
}
