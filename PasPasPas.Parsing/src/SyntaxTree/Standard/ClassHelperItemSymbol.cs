using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper item
    /// </summary>
    public class ClassHelperItemSymbol : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes1 { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes2 { get; set; }

        /// <summary>
        ///     marker for class properties
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public SyntaxPartBase MethodDeclaration { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public SyntaxPartBase PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     variable section
        /// </summary>
        public SyntaxPartBase VarSection { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     constants
        /// </summary>
        public SyntaxPartBase ConstDeclaration { get; set; }

        /// <summary>
        ///     types
        /// </summary>
        public SyntaxPartBase TypeSection { get; set; }

        /// <summary>
        ///     fields
        /// </summary>
        public SyntaxPartBase FieldDeclaration { get; set; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; set; }

        /// <summary>
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; set; }

        /// <summary>
        ///     strict symbol
        /// </summary>
        public Terminal StrictSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes1, visitor);
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, Attributes2, visitor);
            AcceptPart(this, VarSymbol, visitor);
            AcceptPart(this, StrictSymbol, visitor);
            AcceptPart(this, MethodDeclaration, visitor);
            AcceptPart(this, PropertyDeclaration, visitor);
            AcceptPart(this, ConstDeclaration, visitor);
            AcceptPart(this, TypeSection, visitor);
            AcceptPart(this, FieldDeclaration, visitor);
            visitor.EndVisit(this);
        }


        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => Attributes1.Length
            + ClassSymbol.Length
            + Attributes2.Length
            + VarSymbol.Length
            + StrictSymbol.Length
            + MethodDeclaration.Length
            + PropertyDeclaration.Length
            + ConstDeclaration.Length
            + TypeSection.Length
            + FieldDeclaration.Length;
    }
}