using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;
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
            Expressions = new SyntaxPartCollection<IExpression>(this);
            Statements = new BlockOfStatements();
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var expression in Expressions)
                    yield return expression;
                foreach (var statement in Statements.Statements)
                    yield return statement;
            }
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
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
