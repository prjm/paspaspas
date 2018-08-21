using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper item
    /// </summary>
    public class RecordHelperItemSymbol : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     create a new record helper item
        /// </summary>
        /// <param name="varSymbol"></param>
        public RecordHelperItemSymbol(Terminal varSymbol)
            => VarSymbol = varSymbol;

        /// <summary>
        ///     create a new record helper item
        /// </summary>
        /// <param name="constSectionSymbol"></param>
        public RecordHelperItemSymbol(ConstSectionSymbol constSectionSymbol)
            => ConstDeclaration = constSectionSymbol;

        /// <summary>
        ///     create a new record helper item
        /// </summary>
        /// <param name="typeSection"></param>
        public RecordHelperItemSymbol(TypeSection typeSection)
            => TypeSection = typeSection;

        /// <summary>
        ///     create a new record helper item
        /// </summary>
        /// <param name="classPropertySymbol"></param>
        public RecordHelperItemSymbol(ClassPropertySymbol classPropertySymbol)
            => PropertyDeclaration = classPropertySymbol;

        /// <summary>
        ///     create new record helper item
        /// </summary>
        /// <param name="classFieldSymbol"></param>
        public RecordHelperItemSymbol(ClassFieldSymbol classFieldSymbol)
            => FieldDeclaration = classFieldSymbol;

        /// <summary>
        ///     create a new record helper item
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="varSymbol"></param>
        public RecordHelperItemSymbol(Terminal classSymbol, Terminal varSymbol) : this(varSymbol)
            => ClassSymbol = classSymbol;

        /// <summary>
        ///     create a new record helper item
        /// </summary>
        /// <param name="classMethodSymbol"></param>
        /// <param name="classSymbol"></param>
        public RecordHelperItemSymbol(ClassMethodSymbol classMethodSymbol, Terminal classSymbol) {
            ClassSymbol = classSymbol;
            MethodDeclaration = classMethodSymbol;
        }

        /// <summary>
        ///     constant declaration
        /// </summary>
        public ConstSectionSymbol ConstDeclaration { get; }

        /// <summary>
        ///     class flag
        /// </summary>
        public bool ClassItem { get; }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethodSymbol MethodDeclaration { get; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassPropertySymbol PropertyDeclaration { get; }

        /// <summary>
        ///     strict visibility
        /// </summary>
        public bool Strict
            => ClassSymbol.GetSymbolKind() == TokenKind.Strict;

        /// <summary>
        ///     visibility definition
        /// </summary>
        public int Visibility
            => VarSymbol.GetSymbolKind();

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes1 { get; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes2 { get; }


        /// <summary>
        ///     report helper types
        /// </summary>
        public TypeSection TypeSection { get; }

        /// <summary>
        ///     field
        /// </summary>
        public ClassFieldSymbol FieldDeclaration { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

        /// <summary>
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; }

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
            + MethodDeclaration.GetSymbolLength()
            + PropertyDeclaration.GetSymbolLength()
            + ConstDeclaration.GetSymbolLength()
            + TypeSection.GetSymbolLength()
            + FieldDeclaration.GetSymbolLength();

    }
}