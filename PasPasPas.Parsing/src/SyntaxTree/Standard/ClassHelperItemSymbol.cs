using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper item
    /// </summary>
    public class ClassHelperItemSymbol : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     create a new class helper item symbol
        /// </summary>
        /// <param name="varSymbol"></param>
        public ClassHelperItemSymbol(Terminal varSymbol) {
            VarSymbol = varSymbol;
        }

        /// <summary>
        ///     create a new class helper item symbol
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="varSymbol"></param>
        public ClassHelperItemSymbol(Terminal classSymbol, Terminal varSymbol) {
            ClassSymbol = classSymbol;
            VarSymbol = varSymbol;
        }

        /// <summary>
        ///     create a new class helper symbol
        /// </summary>
        /// <param name="constSection"></param>
        public ClassHelperItemSymbol(ConstSection constSection)
            => ConstDeclaration = constSection;

        /// <summary>
        ///     create a new class helper symbol
        /// </summary>
        /// <param name="typeSection"></param>
        public ClassHelperItemSymbol(TypeSection typeSection)
            => TypeSection = typeSection;

        /// <summary>
        ///     create a new class helper symbol
        /// </summary>
        /// <param name="classMethodSymbol"></param>
        public ClassHelperItemSymbol(ClassMethodSymbol classMethodSymbol)
            => MethodDeclaration = classMethodSymbol;

        /// <summary>
        ///     create a new class helper symbol
        /// </summary>
        /// <param name="classPropertySymbol"></param>
        public ClassHelperItemSymbol(ClassPropertySymbol classPropertySymbol)
            => PropertyDeclaration = classPropertySymbol;

        /// <summary>
        ///     create a new class helper item symbol
        /// </summary>
        /// <param name="attributes1"></param>
        /// <param name="classSymbol"></param>
        /// <param name="attributes2"></param>
        /// <param name="strictSymbol"></param>
        /// <param name="visibility"></param>
        public ClassHelperItemSymbol(UserAttributes attributes1, Terminal classSymbol, UserAttributes attributes2, Terminal strictSymbol, Terminal visibility) {
            Attributes1 = attributes1;
            ClassSymbol = classSymbol;
            Attributes2 = attributes2;
            StrictSymbol = strictSymbol;
        }

        /// <summary>
        ///     create a new class helper item
        /// </summary>
        /// <param name="classFieldSymbol"></param>
        public ClassHelperItemSymbol(ClassFieldSymbol classFieldSymbol)
            => FieldDeclaration = classFieldSymbol;

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes1 { get; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes2 { get; }

        /// <summary>
        ///     marker for class properties
        /// </summary>
        public bool ClassItem { get; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethodSymbol MethodDeclaration { get; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public SyntaxPartBase PropertyDeclaration { get; }

        /// <summary>
        ///     strict
        /// </summary>
        public bool Strict { get; }

        /// <summary>
        ///     variable section
        /// </summary>
        public SyntaxPartBase VarSection { get; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; }
            = TokenKind.Undefined;

        /// <summary>
        ///     constants
        /// </summary>
        public SyntaxPartBase ConstDeclaration { get; }

        /// <summary>
        ///     types
        /// </summary>
        public SyntaxPartBase TypeSection { get; }

        /// <summary>
        ///     fields
        /// </summary>
        public SyntaxPartBase FieldDeclaration { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

        /// <summary>
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; }

        /// <summary>
        ///     strict symbol
        /// </summary>
        public Terminal StrictSymbol { get; }

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
        public override int Length
            => Attributes1.GetSymbolLength()
            + ClassSymbol.GetSymbolLength()
            + Attributes2.GetSymbolLength()
            + VarSymbol.GetSymbolLength()
            + StrictSymbol.GetSymbolLength()
            + MethodDeclaration.GetSymbolLength()
            + PropertyDeclaration.GetSymbolLength()
            + ConstDeclaration.GetSymbolLength()
            + TypeSection.GetSymbolLength()
            + FieldDeclaration.GetSymbolLength();
    }
}