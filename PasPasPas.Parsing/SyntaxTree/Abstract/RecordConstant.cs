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
        public ISyntaxPartList<IExpression> Items { get; }

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Items;

        /// <summary>
        ///     record value
        /// </summary>
        public IExpression Value {
            get => Items.LastOrDefault();
            set => Items.Add(value);
        }

        /// <summary>
        ///     create a new record constant
        /// </summary>
        public RecordConstant()
            => Items = new SyntaxPartCollection<IExpression>(this);

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
