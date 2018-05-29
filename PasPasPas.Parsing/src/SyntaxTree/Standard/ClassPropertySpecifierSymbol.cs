using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class property specifier
    /// </summary>
    public class ClassPropertySpecifierSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     default property
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     default property expression
        /// </summary>
        public SyntaxPartBase DefaultProperty { get; set; }

        /// <summary>
        ///     disp interface
        /// </summary>
        public SyntaxPartBase PropertyDispInterface { get; set; }

        /// <summary>
        ///     read write accessors
        /// </summary>
        public SyntaxPartBase PropertyReadWrite { get; set; }

        /// <summary>
        ///     stored property
        /// </summary>
        public bool IsStored { get; set; }

        /// <summary>
        ///     stored property expression
        /// </summary>
        public SyntaxPartBase StoredProperty { get; set; }

        /// <summary>
        ///     no default
        /// </summary>
        public bool NoDefault { get; set; }

        /// <summary>
        ///     implementing type name
        /// </summary>
        public SyntaxPartBase ImplementsTypeId { get; set; }

        /// <summary>
        ///     stored symbol
        /// </summary>
        public Terminal StoredSymbol { get; set; }

        /// <summary>
        ///     default symbol
        /// </summary>
        public Terminal DefaultSymbol { get; set; }

        /// <summary>
        ///     no default
        /// </summary>
        public Terminal NoDefaultSymbol { get; set; }

        /// <summary>
        ///     implements
        /// </summary>
        public Terminal ImplentsSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PropertyReadWrite, visitor);
            AcceptPart(this, PropertyDispInterface, visitor);
            AcceptPart(this, StoredSymbol, visitor);
            AcceptPart(this, StoredProperty, visitor);
            AcceptPart(this, DefaultSymbol, visitor);
            AcceptPart(this, DefaultProperty, visitor);
            AcceptPart(this, NoDefaultSymbol, visitor);
            AcceptPart(this, ImplentsSymbol, visitor);
            AcceptPart(this, ImplementsTypeId, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
               => PropertyReadWrite.Length
                + PropertyDispInterface.Length
                + StoredSymbol.Length
                + StoredProperty.Length
                + DefaultSymbol.Length
                + DefaultProperty.Length
                + NoDefaultSymbol.Length
                + ImplentsSymbol.Length
                + ImplementsTypeId.Length;

    }
}