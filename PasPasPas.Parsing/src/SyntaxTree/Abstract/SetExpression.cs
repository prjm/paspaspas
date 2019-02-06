using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     set expression
    /// </summary>
    public class SetExpression : ExpressionBase, IExpression, IExpressionTarget {

        /// <summary>
        ///     subexpressions
        /// </summary>
        public ISyntaxPartCollection<IExpression> Expressions { get; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => Expressions.LastOrDefault();
            set => Expressions.Add(value);
        }

        /// <summary>
        ///     creates a new set expression
        /// </summary>
        public SetExpression()
            => Expressions = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var part in Expressions)
                    yield return part;

            }
        }

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
