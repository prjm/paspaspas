using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant array value
    /// </summary>
    public class ArrayConstant : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     element items
        /// </summary>
        public ISyntaxPartCollection<IExpression> Items { get; }

        /// <summary>
        ///     Create a new set constant
        /// </summary>
        public ArrayConstant()
            => Items = new SyntaxPartCollection<IExpression>();

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
            AcceptPart(this, Items, visitor);
            visitor.EndVisit(this);
        }
    }
}
