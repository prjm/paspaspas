#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic types
    /// </summary>
    public class GenericTypeCollection : SymbolTableBaseCollection<GenericTypeNameCollection>, ITypeTarget {

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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);

            if (Count > 0) {
                for (var i = 0; i < Count; i++)
                    AcceptPart(this, this[i], visitor);
            }

            visitor.EndVisit(this);
        }

    }
}
