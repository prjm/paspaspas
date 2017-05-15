using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic types
    /// </summary>
    public class GenericTypes : SymbolTableBase<GenericType>, ITypeTarget {

        /// <summary>
        ///     reference to type
        /// </summary>
        public bool TypeReference { get; set; }

        /// <summary>
        ///     type reference
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
