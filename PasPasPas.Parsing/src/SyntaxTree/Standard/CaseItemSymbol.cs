using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case item
    /// </summary>
    public class CaseItemSymbol : VariableLengthSyntaxTreeBase<CaseLabelSymbol> {

        /// <summary>
        ///     case statement
        /// </summary>
        public Statement CaseStatement { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, CaseStatement, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => ItemLength + ColonSymbol.Length + CaseStatement.Length + Semicolon.Length;

    }
}