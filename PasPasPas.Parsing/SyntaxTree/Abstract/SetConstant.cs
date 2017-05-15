using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant array
    /// </summary>
    public class SetConstant : ExpressionBase, IExpressionTarget {

        public ISyntaxPartList<IExpression> Items { get; }

        /// <summary>
        ///     Create a new set constant
        /// </summary>
        public SetConstant()
            => Items = new SyntaxPartCollection<IExpression>(this);

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Items;

        /// <summary>
        ///     value
        /// </summary>
        public IExpression Value {
            get => Items.LastOrDefault();
            set => Items.Add(value);
        }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
