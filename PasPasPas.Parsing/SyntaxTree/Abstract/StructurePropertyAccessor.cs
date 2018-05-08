using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
        ///     enumerate party
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
            }
        }

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
