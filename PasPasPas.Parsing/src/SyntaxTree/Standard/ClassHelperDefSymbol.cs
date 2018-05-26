using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper declaration
    /// </summary>
    public class ClassHelperDefSymbol : StandardSyntaxTreeBase {
        /// <summary>
        ///     items
        /// </summary>
        public ClassHelperItemsSymbol HelperItems { get; set; }

        /// <summary>
        ///     class parent
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     class helper name
        /// </summary>
        public TypeName HelperName { get; set; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; set; }

        /// <summary>
        ///     helper symbol
        /// </summary>
        public Terminal HelperSymbol { get; set; }

        /// <summary>
        ///     for symbol
        /// </summary>
        public Terminal ForSymbol { get; set; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, HelperSymbol, visitor);
            AcceptPart(this, ClassParent, visitor);
            AcceptPart(this, ForSymbol, visitor);
            AcceptPart(this, HelperName, visitor);
            AcceptPart(this, HelperItems, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => ClassSymbol.Length + HelperSymbol.Length + ClassParent.Length + ForSymbol.Length
            + HelperName.Length + HelperItems.Length + EndSymbol.Length;

    }
}