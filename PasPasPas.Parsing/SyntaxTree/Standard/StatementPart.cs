using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     statement part
    /// </summary>
    public class StatementPart : StandardSyntaxTreeBase {

        /// <summary>
        ///     assignment
        /// </summary>
        public Expression Assignment { get; set; }

        /// <summary>
        ///     case statement
        /// </summary>
        public CaseStatement Case { get; set; }

        /// <summary>
        ///     compunt statement
        /// </summary>
        public CompoundStatement CompoundStatement { get; set; }

        /// <summary>
        ///     deisgnator part
        /// </summary>
        public DesignatorStatement DesignatorPart { get; set; }

        /// <summary>
        ///     for statement
        /// </summary>
        public ForStatement For { get; set; }

        /// <summary>
        ///     goto statement
        /// </summary>
        public GoToStatement GoTo { get; set; }

        /// <summary>
        ///     if statement
        /// </summary>
        public IfStatement If { get; set; }

        /// <summary>
        ///     raise statement
        /// </summary>
        public RaiseStatement Raise { get; set; }

        /// <summary>
        ///     repeat statement
        /// </summary>
        public RepeatStatement Repeat { get; set; }

        /// <summary>
        ///     try statement
        /// </summary>
        public TryStatement Try { get; set; }

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


    }
}