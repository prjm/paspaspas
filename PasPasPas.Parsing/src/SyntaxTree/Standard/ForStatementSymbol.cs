using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     create a new for statement
    /// </summary>
    public class ForStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new for statement symbol
        /// </summary>
        /// <param name="forKeyword"></param>
        /// <param name="loopVariable"></param>
        /// <param name="assignment"></param>
        /// <param name="startExpression"></param>
        /// <param name="loopOperator"></param>
        /// <param name="endExpression"></param>
        /// <param name="doKeyword"></param>
        /// <param name="loopStatement"></param>
        public ForStatementSymbol(Terminal forKeyword, IdentifierSymbol loopVariable, Terminal assignment, ExpressionSymbol startExpression, Terminal loopOperator, ExpressionSymbol endExpression, Terminal doKeyword, Statement loopStatement) {
            ForKeyword = forKeyword;
            Variable = loopVariable;
            Assignment = assignment;
            StartExpression = startExpression;
            LoopOperator = loopOperator;
            EndExpression = endExpression;
            DoKeyword = doKeyword;
            Statement = loopStatement;
        }

        /// <summary>
        ///     iteration end
        /// </summary>
        public ExpressionSymbol EndExpression { get; }

        /// <summary>
        ///     iteration kind
        /// </summary>
        public int Kind
            => LoopOperator.GetSymbolKind();

        /// <summary>
        ///     iteration start
        /// </summary>
        public ExpressionSymbol StartExpression { get; }

        /// <summary>
        ///     iteration statement
        /// </summary>
        public Statement Statement { get; }

        /// <summary>
        ///     iteration variable
        /// </summary>
        public IdentifierSymbol Variable { get; }

        /// <summary>
        ///     do keyword
        /// </summary>
        public Terminal DoKeyword { get; }

        /// <summary>
        ///     loop operator
        /// </summary>
        public Terminal LoopOperator { get; }

        /// <summary>
        ///     for keyword
        /// </summary>
        public Terminal ForKeyword { get; }

        /// <summary>
        ///     assignment operator
        /// </summary>
        public Terminal Assignment { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ForKeyword, visitor);
            AcceptPart(this, Variable, visitor);
            AcceptPart(this, Assignment, visitor);
            AcceptPart(this, StartExpression, visitor);
            AcceptPart(this, LoopOperator, visitor);
            AcceptPart(this, EndExpression, visitor);
            AcceptPart(this, DoKeyword, visitor);
            AcceptPart(this, Statement, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ForKeyword.GetSymbolLength() +
                Variable.GetSymbolLength() +
                Assignment.GetSymbolLength() +
                StartExpression.GetSymbolLength() +
                LoopOperator.GetSymbolLength() +
                EndExpression.GetSymbolLength() +
                DoKeyword.GetSymbolLength() +
                Statement.GetSymbolLength();


    }
}