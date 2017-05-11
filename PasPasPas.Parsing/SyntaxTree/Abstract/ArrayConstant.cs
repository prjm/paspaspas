using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant array
    /// </summary>
    public class SetConstant : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     items
        /// </summary>
        public IList<IExpression> Items
            => items;

        private List<IExpression> items
            = new List<IExpression>();

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Items;

        /// <summary>
        ///     value
        /// </summary>
        public IExpression Value {
            get {
                if (items.Count > 0)
                    return items[items.Count - 1];
                else return null;
            }

            set {
                if (value != null)
                    items.Add(value);
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
