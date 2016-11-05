using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant array
    /// </summary>
    public class ArrayConstant : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     items
        /// </summary>
        public IList<ExpressionBase> Items
            => items;

        private List<ExpressionBase> items
            = new List<ExpressionBase>();

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Items;

        /// <summary>
        ///     value
        /// </summary>
        public ExpressionBase Value
        {
            get
            {
                if (items.Count > 0)
                    return items[items.Count - 1];
                else return null;
            }

            set
            {
                if (value != null)
                    items.Add(value);
            }
        }
    }
}
