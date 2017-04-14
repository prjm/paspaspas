using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : AbstractSyntaxPart, IExpressionTarget, IExpression {

        /// <summary>
        ///     expressions
        /// </summary>
        public IList<IExpression> Expressions { get; }
            = new List<IExpression>();

        /// <summary>
        ///     expressiona
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
            }
        }
    }
}
