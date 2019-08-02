using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     set expression
    /// </summary>
    public class SetExpression : ExpressionBase, IExpression, IExpressionTarget, IRequiresArrayExpression {

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
        ///     <c>true</c> if this set expression is used to create an modern-style array
        /// </summary>
        public bool RequiresArray { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to accept</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Expressions, visitor);
            visitor.EndVisit(this);
        }
    }
}
