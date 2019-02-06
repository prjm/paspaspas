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
        public ISyntaxPartCollection<IExpression> Items { get; }

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
            => Items = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
