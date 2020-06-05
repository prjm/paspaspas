#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : ExpressionBase, IExpressionTarget, IExpression {

        /// <summary>
        ///     expressions
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
        ///     create a new formatted expression
        /// </summary>
        public FormattedExpression()
            => Expressions = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Expressions, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     check if this expression is constant
        /// </summary>
        public bool IsConstant {
            get {
                for (var i = 0; i < Expressions.Count; i++) {

                    var expression = Expressions[i];

                    if (expression.TypeInfo == default)
                        return false;

                    if (!expression.TypeInfo.IsConstant())
                        return false;
                }

                return true;
            }
        }

    }
}
