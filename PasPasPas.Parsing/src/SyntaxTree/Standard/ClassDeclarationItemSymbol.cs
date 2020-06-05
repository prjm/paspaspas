#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration item
    /// </summary>
    public class ClassDeclarationItemSymbol : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     create a new class declaration item
        /// </summary>
        /// <param name="varSymbol"></param>
        public ClassDeclarationItemSymbol(Terminal varSymbol) => VarSymbol = varSymbol;

        /// <summary>
        ///     create a new class declaration item
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="varSymbol"></param>
        public ClassDeclarationItemSymbol(Terminal classSymbol, Terminal varSymbol) {
            ClassSymbol = classSymbol;
            VarSymbol = varSymbol;
        }

        /// <summary>
        ///     create a new class declaration symbol
        /// </summary>
        /// <param name="attributes1"></param>
        /// <param name="classSymbol"></param>
        /// <param name="attributes2"></param>
        /// <param name="strictSymbol"></param>
        /// <param name="visibility"></param>
        public ClassDeclarationItemSymbol(UserAttributesSymbol attributes1, Terminal classSymbol, UserAttributesSymbol attributes2, Terminal strictSymbol, Terminal visibility) {
            Attributes1 = attributes1;
            ClassSymbol = classSymbol;
            Attributes2 = attributes2;
            StrictSymbol = strictSymbol;
            VisibilitySymbol = visibility;
        }

        /// <summary>
        ///     create a new method declaration
        /// </summary>
        /// <param name="classMethodSymbol"></param>
        /// <param name="attributes1"></param>
        /// <param name="attributes2"></param>
        /// <param name="classSymbol"></param>
        public ClassDeclarationItemSymbol(ClassMethodSymbol classMethodSymbol, UserAttributesSymbol attributes1, UserAttributesSymbol attributes2, Terminal classSymbol) {
            MethodDeclaration = classMethodSymbol;
            Attributes1 = attributes1;
            Attributes2 = attributes2;
            ClassSymbol = classSymbol;
        }

        /// <summary>
        ///     create a new class property symbol
        /// </summary>
        /// <param name="classPropertySymbol"></param>
        /// <param name="attributes1"></param>
        /// <param name="attributes2"></param>
        /// <param name="classSymbol"></param>
        public ClassDeclarationItemSymbol(ClassPropertySymbol classPropertySymbol, UserAttributesSymbol attributes1, UserAttributesSymbol attributes2, Terminal classSymbol) {
            PropertyDeclaration = classPropertySymbol;
            Attributes1 = attributes1;
            Attributes2 = attributes2;
            ClassSymbol = classSymbol;
        }

        /// <summary>
        ///     create a new class declaration item symbol
        /// </summary>
        /// <param name="constSection"></param>
        public ClassDeclarationItemSymbol(ConstSectionSymbol constSection)
            => ConstSection = constSection;

        /// <summary>
        ///     create a new class type section
        /// </summary>
        /// <param name="typeSection"></param>
        public ClassDeclarationItemSymbol(TypeSectionSymbol typeSection)
            => TypeSection = typeSection;

        /// <summary>
        ///     create a new method resolution
        /// </summary>
        /// <param name="methodResolution"></param>
        public ClassDeclarationItemSymbol(MethodResolutionSymbol methodResolution)
            => MethodResolution = methodResolution;

        /// <summary>
        ///     create a new class field declaration
        /// </summary>
        /// <param name="classFieldSymbol"></param>
        /// <param name="classItem"></param>
        /// <param name="attributes1"></param>
        /// <param name="attributes2"></param>
        public ClassDeclarationItemSymbol(ClassFieldSymbol classFieldSymbol, bool classItem, UserAttributesSymbol attributes1, UserAttributesSymbol attributes2) {
            FieldDeclaration = classFieldSymbol;
            ClassFieldItem = classItem;
            Attributes1 = attributes1;
            Attributes2 = attributes2;
        }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributesSymbol Attributes1 { get; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributesSymbol Attributes2 { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

        /// <summary>
        ///     class-wide declaration
        /// </summary>
        public bool ClassItem
            => ClassSymbol != null || ClassFieldItem;

        /// <summary>
        ///     class field item
        /// </summary>
        public bool ClassFieldItem { get; }

        /// <summary>
        ///     constant class section
        /// </summary>
        public ConstSectionSymbol ConstSection { get; }

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassFieldSymbol FieldDeclaration { get; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethodSymbol MethodDeclaration { get; }

        /// <summary>
        ///     method resolution
        /// </summary>
        public MethodResolutionSymbol MethodResolution { get; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassPropertySymbol PropertyDeclaration { get; }

        /// <summary>
        ///     strict declaration
        /// </summary>
        public bool Strict
            => StrictSymbol.GetSymbolKind() == TokenKind.Strict;

        /// <summary>
        ///     type section
        /// </summary>
        public TypeSectionSymbol TypeSection { get; }

        /// <summary>
        ///     visibility declaration
        /// </summary>
        public int Visibility
            => VisibilitySymbol == default ? TokenKind.Undefined : VisibilitySymbol.Kind;

        /// <summary>
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; }

        /// <summary>
        ///     strict symbol
        /// </summary>
        public Terminal StrictSymbol { get; }

        /// <summary>
        ///     visibility
        /// </summary>
        public Terminal VisibilitySymbol { get; }

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
            AcceptPart(this, VisibilitySymbol, visitor);
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
        public override int Length
            => Attributes1.GetSymbolLength()
            + ClassSymbol.GetSymbolLength()
            + Attributes2.GetSymbolLength()
            + VarSymbol.GetSymbolLength()
            + StrictSymbol.GetSymbolLength()
            + VisibilitySymbol.GetSymbolLength()
            + MethodResolution.GetSymbolLength()
            + MethodDeclaration.GetSymbolLength()
            + PropertyDeclaration.GetSymbolLength()
            + ConstSection.GetSymbolLength()
            + TypeSection.GetSymbolLength()
            + FieldDeclaration.GetSymbolLength();

    }
}