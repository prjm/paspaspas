using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     record constant
    /// </summary>
    public class RecordConstant : ExpressionBase {


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


    }
}
