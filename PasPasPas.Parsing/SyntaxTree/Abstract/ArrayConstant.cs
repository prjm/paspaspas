using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant array
    /// </summary>
    public class ArrayConstant : ExpressionBase {

        /// <summary>
        ///     items
        /// </summary>
        public IList<ArrayConstantItem> Items { get; }
        = new List<ArrayConstantItem>();

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Items;

    }
}
