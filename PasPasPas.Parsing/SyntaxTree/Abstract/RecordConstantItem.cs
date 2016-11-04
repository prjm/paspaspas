
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     record constant item
    /// </summary>
    public class RecordConstantItem : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     constant item value
        /// </summary>
        public ExpressionBase Value { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (Value != null)
                    yield return Value;
            }
        }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }
    }
}
