using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     item in a variant structure
    /// </summary>
    public class StructureVariantItem : AbstractSyntaxPart, ITypeTarget {

        /// <summary>
        ///     variant name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     fields
        /// </summary>
        public IList<StructureVariantFields> Items { get; }
            = new List<StructureVariantFields>();


        /// <summary>
        ///     type target
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
                foreach (StructureVariantFields field in Items)
                    yield return field;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}