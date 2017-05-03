using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     fields
    /// </summary>
    public class StructureFields : AbstractSyntaxPart, ITypeTarget {

        /// <summary>
        ///     list of fields
        /// </summary>
        public IList<StructureField> Fields { get; }
            = new List<StructureField>();

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (StructureField field in Fields)
                    yield return field;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

        /// <summary>
        ///     field type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }
            = MemberVisibility.Public;

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     <c>true</c> for class variables
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
