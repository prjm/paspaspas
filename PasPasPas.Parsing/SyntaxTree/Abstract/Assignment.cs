using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     assignment statements
    /// </summary>
    public class Assignment : StatementBase, IExpressionTarget {

        /// <summary>
        ///     assignment expression 
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
            }
        }

    }
}
