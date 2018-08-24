using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     statement part
    /// </summary>
    public class StatementPart : StandardSyntaxTreeBase {

        /// <summary>
        ///     assignment
        /// </summary>
        public ExpressionSymbol Assignment { get; set; }

        /// <summary>
        ///     case statement
        /// </summary>
        public CaseStatementSymbol Case { get; set; }

        /// <summary>
        ///     compunt statement
        /// </summary>
        public CompoundStatementSymbol CompoundStatement { get; set; }

        /// <summary>
        ///     deisgnator part
        /// </summary>
        public DesignatorStatementSymbol DesignatorPart { get; set; }

        /// <summary>
        ///     for statement
        /// </summary>
        public ForStatementSymbol For { get; set; }

        /// <summary>
        ///     goto statement
        /// </summary>
        public GoToStatementSymbol GoTo { get; set; }

        /// <summary>
        ///     if statement
        /// </summary>
        public IfStatementSymbol If { get; set; }

        /// <summary>
        ///     raise statement
        /// </summary>
        public RaiseStatementSymbol Raise { get; set; }

        /// <summary>
        ///     repeat statement
        /// </summary>
        public RepeatStatement Repeat { get; set; }

        /// <summary>
        ///     try statement
        /// </summary>
        public TryStatementSymbol Try { get; set; }

        /// <summary>
        ///     while statement
        /// </summary>
        public WhileStatement While { get; set; }

        /// <summary>
        ///     with statement
        /// </summary>
        public WithStatement With { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length =>
            Assignment.GetSymbolLength() +
            Case.GetSymbolLength() +
            CompoundStatement.GetSymbolLength() +
            DesignatorPart.GetSymbolLength() +
            For.GetSymbolLength() +
            GoTo.GetSymbolLength() +
            If.GetSymbolLength() +
            Raise.GetSymbolLength() +
            Repeat.GetSymbolLength() +
            Try.GetSymbolLength() +
            While.GetSymbolLength() +
            With.GetSymbolLength();

    }
}