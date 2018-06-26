using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case statement
    /// </summary>
    public class CaseStatementSymbol : VariableLengthSyntaxTreeBase<CaseItemSymbol> {

        /// <summary>
        ///     case item symbol
        /// </summary>
        /// <param name="items"></param>
        public CaseStatementSymbol(ImmutableArray<CaseItemSymbol> items) : base(items) {

        }

        /// <summary>
        ///     case expression
        /// </summary>
        public Expression CaseExpression { get; set; }

        /// <summary>
        ///     else part
        /// </summary>
        public SyntaxPartBase Else { get; set; }

        /// <summary>
        ///     case symbol
        /// </summary>
        public Terminal CaseSymbol { get; set; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     else symbol
        /// </summary>
        public Terminal ElseSymbol { get; set; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, CaseSymbol, visitor);
            AcceptPart(this, CaseExpression, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, ElseSymbol, visitor);
            AcceptPart(this, Else, visitor);
            AcceptPart(this, Semicolon, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => CaseSymbol.Length + CaseExpression.Length + OfSymbol.Length + ItemLength + ElseSymbol.Length + Else.Length + Semicolon.Length + EndSymbol.Length;
    }
}