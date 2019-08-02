using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     record constant
    /// </summary>
    public class RecordConstant : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     items
        /// </summary>
        public ISyntaxPartCollection<IExpression> Items { get; }

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
            => Items = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Items, visitor);
            visitor.EndVisit(this);
        }
    }
}
