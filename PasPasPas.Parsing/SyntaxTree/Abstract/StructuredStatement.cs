using System.Linq;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structured statement
    /// </summary>
    public class StructuredStatement : StatementBase, IExpressionTarget, IStatementTarget {

        /// <summary>
        ///     create a a new structured Statement
        /// </summary>
        public StructuredStatement() {
            Statements = new BlockOfStatements() { Parent = this };
        }

        /// <summary>
        ///     expressions
        /// </summary>
        public IList<IExpression> Expressions { get; }
            = new List<IExpression>();

        /// <summary>
        ///     statement kind
        /// </summary>
        public StructuredStatementKind Kind { get; set; }

        /// <summary>
        ///     expressions
        /// </summary>
        public IExpression Value {
            get {
                return Expressions.LastOrDefault();
            }

            set {
                Expressions.Add(value);
            }
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (IExpression expression in Expressions)
                    yield return expression;
                foreach (StatementBase statement in Statements.Statements)
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
    }
}
