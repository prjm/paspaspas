using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     statement part
    /// </summary>
    public class StatementPart : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="ifStatementSymbol"></param>
        public StatementPart(IfStatementSymbol ifStatementSymbol)
            => If = ifStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="caseStatementSymbol"></param>
        public StatementPart(CaseStatementSymbol caseStatementSymbol)
            => Case = caseStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="repeatStatement"></param>
        public StatementPart(RepeatStatement repeatStatement)
            => Repeat = repeatStatement;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="whileStatementSymbol"></param>
        public StatementPart(WhileStatementSymbol whileStatementSymbol)
            => While = whileStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="forStatementSymbol"></param>
        public StatementPart(ForStatementSymbol forStatementSymbol)
            => For = forStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="withStatementSymbol"></param>
        public StatementPart(WithStatementSymbol withStatementSymbol)
            => With = withStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="tryStatementSymbol"></param>
        public StatementPart(TryStatementSymbol tryStatementSymbol)
            => Try = tryStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="raiseStatementSymbol"></param>
        public StatementPart(RaiseStatementSymbol raiseStatementSymbol)
            => Raise = raiseStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="compoundStatementSymbol"></param>
        public StatementPart(CompoundStatementSymbol compoundStatementSymbol)
            => CompoundStatement = compoundStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="goToStatementSymbol"></param>
        public StatementPart(GoToStatementSymbol goToStatementSymbol)
            => GoTo = goToStatementSymbol;

        /// <summary>
        ///     create a new statement part
        /// </summary>
        /// <param name="designator"></param>
        /// <param name="assignmentSymbol"></param>
        /// <param name="assignmentValue"></param>
        public StatementPart(DesignatorStatementSymbol designator, Terminal assignmentSymbol, ExpressionSymbol assignmentValue) {
            DesignatorPart = designator;
            AssignmentSymbol = assignmentSymbol;
            Assignment = assignmentValue;
        }

        /// <summary>
        ///     assignment symbol
        /// </summary>
        public Terminal AssignmentSymbol { get; }

        /// <summary>
        ///     assignment
        /// </summary>
        public ExpressionSymbol Assignment { get; }

        /// <summary>
        ///     case statement
        /// </summary>
        public CaseStatementSymbol Case { get; }

        /// <summary>
        ///     compound statement
        /// </summary>
        public CompoundStatementSymbol CompoundStatement { get; }

        /// <summary>
        ///     designator part
        /// </summary>
        public DesignatorStatementSymbol DesignatorPart { get; }

        /// <summary>
        ///     for statement
        /// </summary>
        public ForStatementSymbol For { get; }

        /// <summary>
        ///     goto statement
        /// </summary>
        public GoToStatementSymbol GoTo { get; }

        /// <summary>
        ///     if statement
        /// </summary>
        public IfStatementSymbol If { get; }

        /// <summary>
        ///     raise statement
        /// </summary>
        public RaiseStatementSymbol Raise { get; }

        /// <summary>
        ///     repeat statement
        /// </summary>
        public RepeatStatement Repeat { get; }

        /// <summary>
        ///     try statement
        /// </summary>
        public TryStatementSymbol Try { get; }

        /// <summary>
        ///     while statement
        /// </summary>
        public WhileStatementSymbol While { get; }

        /// <summary>
        ///     with statement
        /// </summary>
        public WithStatementSymbol With { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Assignment, visitor);
            AcceptPart(this, Case, visitor);
            AcceptPart(this, CompoundStatement, visitor);
            AcceptPart(this, DesignatorPart, visitor);
            AcceptPart(this, For, visitor);
            AcceptPart(this, GoTo, visitor);
            AcceptPart(this, If, visitor);
            AcceptPart(this, Raise, visitor);
            AcceptPart(this, Repeat, visitor);
            AcceptPart(this, Try, visitor);
            AcceptPart(this, While, visitor);
            AcceptPart(this, With, visitor);
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