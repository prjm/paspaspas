using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic type
    /// </summary>
    public class GenericTypeNameCollection : SymbolTableBaseCollection<GenericConstraint>, ISymbolTableEntry, ITypedSyntaxPart {

        /// <summary>
        ///     type name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public string SymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     type inf
        /// </summary>
        public ITypeSymbol TypeInfo { get; set; }

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