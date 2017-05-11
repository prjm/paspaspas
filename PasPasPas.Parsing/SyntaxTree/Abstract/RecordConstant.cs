using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
        public IExpression Value {

            get {
                if (Items.Count > 0)
                    return Items[Items.Count - 1];
                else
                    return null;
            }

            set {
                var item = value as RecordConstantItem;
                if (item != null)
                    Items.Add(item);
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
