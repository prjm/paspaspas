using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case statement
    /// </summary>
    public class CaseStatementSymbol : VariableLengthSyntaxTreeBase<CaseItemSymbol> {

        /// <summary>
        ///     create a new case statement
        /// </summary>
        /// <param name="caseSymbol"></param>
        /// <param name="caseExpression"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="items"></param>
        /// <param name="endSymbol"></param>
        public CaseStatementSymbol(Terminal caseSymbol, ExpressionSymbol caseExpression, Terminal ofSymbol, ImmutableArray<CaseItemSymbol> items, Terminal endSymbol) : base(items) {
            CaseSymbol = caseSymbol;
            CaseExpression = caseExpression;
            OfSymbol = ofSymbol;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     create a new case statement
        /// </summary>
        /// <param name="caseSymbol"></param>
        /// <param name="caseExpression"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="items"></param>
        /// <param name="elseSymbol"></param>
        /// <param name="endSymbol"></param>
        /// <param name="elsePart"></param>
        /// <param name="semicolon"></param>
        public CaseStatementSymbol(Terminal caseSymbol, ExpressionSymbol caseExpression, Terminal ofSymbol, ImmutableArray<CaseItemSymbol> items, Terminal elseSymbol, StatementList elsePart, Terminal semicolon, Terminal endSymbol) : base(items) {
            CaseSymbol = caseSymbol;
            CaseExpression = caseExpression;
            OfSymbol = ofSymbol;
            ElseSymbol = elseSymbol;
            Else = elsePart;
            Semicolon = semicolon;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     case expression
        /// </summary>
        public ExpressionSymbol CaseExpression { get; }

        /// <summary>
        ///     else part
        /// </summary>
        public StatementList Else { get; }

        /// <summary>
        ///     case symbol
        /// </summary>
        public Terminal CaseSymbol { get; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     else symbol
        /// </summary>
        public Terminal ElseSymbol { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

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
        public override int Length
            => CaseSymbol.GetSymbolLength() +
               CaseExpression.GetSymbolLength() +
               OfSymbol.GetSymbolLength() +
               ItemLength +
               ElseSymbol.GetSymbolLength() +
               Else.GetSymbolLength() +
               Semicolon.GetSymbolLength() +
               EndSymbol.GetSymbolLength();
    }
}