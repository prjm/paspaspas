using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     repeat statement
    /// </summary>
    public class RepeatStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new repeat statement
        /// </summary>
        /// <param name="repeat"></param>
        /// <param name="statements"></param>
        /// <param name="until"></param>
        /// <param name="condition"></param>
        public RepeatStatement(Terminal repeat, StatementList statements, Terminal until, ExpressionSymbol condition) {
            Repeat = repeat;
            Statements = statements;
            Until = until;
            Condition = condition;
        }

        /// <summary>
        ///     condition
        /// </summary>
        public ExpressionSymbol Condition { get; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; }

        /// <summary>
        ///     repeat symbol
        /// </summary>
        public Terminal Repeat { get; }

        /// <summary>
        ///     until symbol
        /// </summary>
        public Terminal Until { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Repeat, visitor);
            AcceptPart(this, Statements, visitor);
            AcceptPart(this, Until, visitor);
            AcceptPart(this, Condition, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Repeat.GetSymbolLength() +
                Statements.GetSymbolLength() +
                Until.GetSymbolLength() +
                Condition.GetSymbolLength();
    }
}