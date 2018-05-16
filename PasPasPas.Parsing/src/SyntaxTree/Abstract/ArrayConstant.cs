using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant array
    /// </summary>
    public class ArrayConstant : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     element items
        /// </summary>
        public ISyntaxPartList<IExpression> Items { get; }

        /// <summary>
        ///     Create a new set constant
        /// </summary>
        public ArrayConstant()
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
        /// <param name="visitor">visitor to accept</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
