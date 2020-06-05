#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class StructureMethodResolution : AbstractSyntaxPartBase, ITypeTarget {

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     method kind
        /// </summary>
        public StructureMethodResolutionKind Kind { get; set; }

        /// <summary>
        ///     target identifier
        /// </summary>
        public SymbolName Target { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeValue, visitor);
            visitor.EndVisit(this);
        }
    }
}
