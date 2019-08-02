using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     property accessor kind
    /// </summary>
    public class StructurePropertyAccessor : AbstractSyntaxPartBase, IExpressionTarget {

        /// <summary>
        ///     accessor kind
        /// </summary>
        public StructurePropertyAccessorKind Kind { get; set; }

        /// <summary>
        ///     accessor member name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     expression for disp ids
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }
    }
}
