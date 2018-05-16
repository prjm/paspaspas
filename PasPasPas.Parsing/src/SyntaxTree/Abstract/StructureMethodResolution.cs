using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

        /// <summary>
        ///     attributes
        /// </summary>                                             
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     methodkind
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
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
