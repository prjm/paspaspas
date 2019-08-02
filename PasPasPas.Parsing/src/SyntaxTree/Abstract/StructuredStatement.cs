using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structured statement
    /// </summary>
    public class StructuredStatement : StatementBase, IExpressionTarget, IStatementTarget, ILabelTarget, ITypeTarget {

        /// <summary>
        ///     expressions
        /// </summary>
        public ISyntaxPartCollection<IExpression> Expressions { get; }

        /// <summary>
        ///     statement kind
        /// </summary>
        public StructuredStatementKind Kind { get; set; }

        /// <summary>
        ///     expressions
        /// </summary>
        public IExpression Value {
            get => Expressions.LastOrDefault();
            set => Expressions.Add(value);
        }

        /// <summary>
        ///     create a new structured statement
        /// </summary>
        public StructuredStatement() {
            Expressions = new SyntaxPartCollection<IExpression>();
            Statements = new BlockOfStatements();
        }

        /// <summary>
        ///     statements
        /// </summary>
        public BlockOfStatements Statements { get; }

        /// <summary>
        ///     name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to accept</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Expressions, visitor);
            AcceptPart(this, Statements.Statements, visitor);
            visitor.EndVisit(this);
        }
    }
}
