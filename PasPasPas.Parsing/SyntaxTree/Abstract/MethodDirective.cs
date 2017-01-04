using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method directive
    /// </summary>
    public class MethodDirective : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     kind
        /// </summary>
        public MethodDirectiveKind Kind { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public ExpressionBase Value { get; set; }

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
            }
        }

    }
}
