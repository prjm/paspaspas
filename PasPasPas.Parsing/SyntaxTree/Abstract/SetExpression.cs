using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     set expression
    /// </summary>
    public class SetExpression : AbstractSyntaxPart, IExpression, IExpressionTarget {

        /// <summary>
        ///     subexpressions
        /// </summary>
        public IList<IExpression> Expressions { get; } =
            new List<IExpression>();

        /// <summary>
        ///     expression value
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
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (IExpression part in Expressions)
                    yield return part;

            }
        }
    }
}
