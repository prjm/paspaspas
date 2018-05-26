using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration item
    /// </summary>
    public class ClassDeclarationItemSymbol : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes1 { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes2 { get; set; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; set; }

        /// <summary>
        ///     class-wide declaration
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     constant class section
        /// </summary>
        public SyntaxPartBase ConstSection { get; set; }

        /// <summary>
        ///     field declaration
        /// </summary>
        public SyntaxPartBase FieldDeclaration { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public SyntaxPartBase MethodDeclaration { get; set; }

        /// <summary>
        ///     method resolution
        /// </summary>
        public SyntaxPartBase MethodResolution { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public SyntaxPartBase PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict declaration
        /// </summary>
        public bool Strict
            => StrictSymbol.Kind == TokenKind.Strict;

        /// <summary>
        ///     type section
        /// </summary>
        public SyntaxPartBase TypeSection { get; set; }

        /// <summary>
        ///     visibility declaration
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes1, visitor);
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, Attributes2, visitor);
            AcceptPart(this, VarSymbol, visitor);
            AcceptPart(this, StrictSymbol, visitor);
            AcceptPart(this, MethodResolution, visitor);
            AcceptPart(this, MethodDeclaration, visitor);
            AcceptPart(this, PropertyDeclaration, visitor);
            AcceptPart(this, ConstSection, visitor);
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
            + MethodResolution.Length
            + MethodDeclaration.Length
            + PropertyDeclaration.Length
            + ConstSection.Length
            + TypeSection.Length
            + FieldDeclaration.Length;

    }
}