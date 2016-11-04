using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     record constant
    /// </summary>
    public class RecordConstant : ExpressionBase, IExpressionTarget {


        /// <summary>
        ///     items
        /// </summary>
        public IList<RecordConstantItem> Items { get; }
        = new List<RecordConstantItem>();

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Items;

        /// <summary>
        ///     record value
        /// </summary>
        public ExpressionBase Value
        {
            get
            {
                if (Items.Count > 0)
                    return Items[Items.Count - 1];
                else
                    return null;
            }

            set
            {
                var item = value as RecordConstantItem;
                if (item != null)
                    Items.Add(item);
            }
        }
    }
}
