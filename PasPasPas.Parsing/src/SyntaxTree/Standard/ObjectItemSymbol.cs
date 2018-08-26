using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     object item
    /// </summary>
    public class ObjectItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///    create a new object item
        /// </summary>
        /// <param name="classMethodSymbol"></param>
        public ObjectItem(ClassMethodSymbol classMethodSymbol)
            => MethodDeclaration = classMethodSymbol;

        /// <summary>
        ///     create a new object item
        /// </summary>
        /// <param name="classPropertySymbol"></param>
        public ObjectItem(ClassPropertySymbol classPropertySymbol)
            => Property = classPropertySymbol;

        /// <summary>
        ///     create a new object item
        /// </summary>
        /// <param name="constSectionSymbol"></param>
        public ObjectItem(ConstSectionSymbol constSectionSymbol)
            => ConstSection = constSectionSymbol;

        /// <summary>
        ///     create a new object item
        /// </summary>
        /// <param name="typeSection"></param>
        public ObjectItem(TypeSectionSymbol typeSection)
            => TypeSection = typeSection;

        /// <summary>
        ///     create a new object item
        /// </summary>
        /// <param name="classFieldSymbol"></param>
        public ObjectItem(ClassFieldSymbol classFieldSymbol)
            => FieldDeclaration = classFieldSymbol;

        /// <summary>
        ///     object item
        /// </summary>
        /// <param name="varSymbol"></param>
        public ObjectItem(Terminal varSymbol)
            => VarSymbol = varSymbol;

        /// <summary>
        ///     create a new object item
        /// </summary>
        /// <param name="strictSymbol"></param>
        /// <param name="visibility"></param>
        public ObjectItem(Terminal strictSymbol, Terminal visibility) {
            Strict = strictSymbol;
            Visibility = visibility;
        }

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassFieldSymbol FieldDeclaration { get; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethodSymbol MethodDeclaration { get; }

        /// <summary>
        ///     strict modifier
        /// </summary>
        public Terminal Strict { get; }

        /// <summary>
        ///     visibility
        /// </summary>
        public Terminal Visibility { get; }

        /// <summary>
        ///     class item
        /// </summary>
        public bool ClassItem
            => false;

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributesSymbol Attributes1 { get; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributesSymbol Attributes2 { get; }

        /// <summary>
        ///     properties
        /// </summary>
        public ClassPropertySymbol Property { get; }

        /// <summary>
        ///     type section
        /// </summary>
        public TypeSectionSymbol TypeSection { get; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSectionSymbol ConstSection { get; }

        /// <summary>
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes1, visitor);
            AcceptPart(this, Attributes2, visitor);
            AcceptPart(this, VarSymbol, visitor);
            AcceptPart(this, Strict, visitor);
            AcceptPart(this, Visibility, visitor);
            AcceptPart(this, MethodDeclaration, visitor);
            AcceptPart(this, Property, visitor);
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
            + Attributes2.GetSymbolLength()
            + VarSymbol.GetSymbolLength()
            + Strict.GetSymbolLength()
            + Visibility.GetSymbolLength()
            + MethodDeclaration.GetSymbolLength()
            + Property.GetSymbolLength()
            + ConstSection.GetSymbolLength()
            + TypeSection.GetSymbolLength()
            + FieldDeclaration.GetSymbolLength();

    }
}
