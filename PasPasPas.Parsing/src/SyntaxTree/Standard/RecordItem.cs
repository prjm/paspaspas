using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record item
    /// </summary>
    public class RecordItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     create a new record item
        /// </summary>
        /// <param name="varSymbol"></param>
        public RecordItem(Terminal varSymbol)
            => VarSymbol = varSymbol;

        /// <summary>
        ///     create a new method symbol
        /// </summary>
        /// <param name="classMethodSymbol"></param>
        public RecordItem(ClassMethodSymbol classMethodSymbol)
            => MethodDeclaration = classMethodSymbol;

        /// <summary>
        ///     create a new record property
        /// </summary>
        /// <param name="classPropertySymbol"></param>
        public RecordItem(ClassPropertySymbol classPropertySymbol)
            => PropertyDeclaration = classPropertySymbol;

        /// <summary>
        ///     create a new record variant section
        /// </summary>
        /// <param name="recordVariantSection"></param>
        public RecordItem(RecordVariantSection recordVariantSection)
            => VariantSection = recordVariantSection;

        /// <summary>
        ///     create a new record constant
        /// </summary>
        /// <param name="constSectionSymbol"></param>
        public RecordItem(ConstSectionSymbol constSectionSymbol)
            => ConstSection = constSectionSymbol;

        /// <summary>
        ///     create a new record section
        /// </summary>
        /// <param name="typeSection"></param>
        public RecordItem(TypeSection typeSection) => TypeSection = typeSection;

        /// <summary>
        ///     create a new record field item
        /// </summary>
        /// <param name="recordFieldList"></param>
        public RecordItem(RecordFieldList recordFieldList)
            => Fields = recordFieldList;

        /// <summary>
        ///     create a new record item
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="varSymbol"></param>
        public RecordItem(Terminal classSymbol, Terminal varSymbol) : this(varSymbol)
            => ClassSymbol = varSymbol;

        /// <summary>
        ///     class item
        /// </summary>
        public bool ClassItem { get; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSectionSymbol ConstSection { get; }

        /// <summary>
        ///     record fields
        /// </summary>
        public RecordFieldList Fields { get; }

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
        ///     type
        /// </summary>
        public TypeSection TypeSection { get; }

        /// <summary>
        ///     record variant section
        /// </summary>
        public RecordVariantSection VariantSection { get; }

        /// <summary>
        ///     visibility declaration
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
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

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
            AcceptPart(this, VariantSection, visitor);
            AcceptPart(this, MethodDeclaration, visitor);
            AcceptPart(this, PropertyDeclaration, visitor);
            AcceptPart(this, ConstSection, visitor);
            AcceptPart(this, TypeSection, visitor);
            AcceptPart(this, Fields, visitor);
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
            + VariantSection.GetSymbolLength()
            + MethodDeclaration.GetSymbolLength()
            + PropertyDeclaration.GetSymbolLength()
            + ConstSection.GetSymbolLength()
            + TypeSection.GetSymbolLength()
            + Fields.GetSymbolLength();


    }
}