using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class type declaration
    /// </summary>
    public class ClassTypeDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new class type declaration symbol
        /// </summary>
        /// <param name="classOfDeclarationSymbol"></param>
        public ClassTypeDeclarationSymbol(ClassOfDeclarationSymbol classOfDeclarationSymbol)
            => ClassOf = classOfDeclarationSymbol;

        /// <summary>
        ///     create a new class type declaration symbol
        /// </summary>
        /// <param name="classHelperDefSymbol"></param>
        public ClassTypeDeclarationSymbol(ClassHelperDefSymbol classHelperDefSymbol)
            => ClassHelper = classHelperDefSymbol;

        /// <summary>
        ///     create a new class type declaration symbol
        /// </summary>
        /// <param name="classDeclarationSymbol"></param>
        public ClassTypeDeclarationSymbol(ClassDeclarationSymbol classDeclarationSymbol)
            => ClassDef = classDeclarationSymbol;

        /// <summary>
        ///     create a new class type declaration symbol
        /// </summary>
        /// <param name="interfaceDefinition"></param>
        public ClassTypeDeclarationSymbol(InterfaceDefinitionSymbol interfaceDefinition)
            => InterfaceDef = interfaceDefinition;

        /// <summary>
        ///     create a new class type declaration symbol
        /// </summary>
        /// <param name="objectDeclaration"></param>
        public ClassTypeDeclarationSymbol(ObjectDeclaration objectDeclaration)
            => ObjectDecl = objectDeclaration;

        /// <summary>
        ///     create a new class type declaration symbol
        /// </summary>
        /// <param name="recordHelperDefinition"></param>
        public ClassTypeDeclarationSymbol(RecordHelperDefinition recordHelperDefinition)
            => RecordHelper = recordHelperDefinition;

        /// <summary>
        ///     create a new class declaration symbol
        /// </summary>
        /// <param name="recordDeclaration"></param>
        public ClassTypeDeclarationSymbol(RecordDeclaration recordDeclaration)
            => RecordDecl = recordDeclaration;

        /// <summary>
        ///     class declaration
        /// </summary>
        public ClassDeclarationSymbol ClassDef { get; }

        /// <summary>
        ///     class helper
        /// </summary>
        public ClassHelperDefSymbol ClassHelper { get; }

        /// <summary>
        ///     class of declaration
        /// </summary>
        public ClassOfDeclarationSymbol ClassOf { get; }

        /// <summary>
        ///     interface definition
        /// </summary>
        public InterfaceDefinitionSymbol InterfaceDef { get; }

        /// <summary>
        ///     object declaration
        /// </summary>
        public ObjectDeclaration ObjectDecl { get; }

        /// <summary>
        ///     record declaration
        /// </summary>
        public RecordDeclaration RecordDecl { get; }

        /// <summary>
        ///     record helper
        /// </summary>
        public RecordHelperDefinition RecordHelper { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClassOf, visitor);
            AcceptPart(this, ClassHelper, visitor);
            AcceptPart(this, ClassDef, visitor);
            AcceptPart(this, InterfaceDef, visitor);
            AcceptPart(this, ObjectDecl, visitor);
            AcceptPart(this, RecordDecl, visitor);
            AcceptPart(this, RecordHelper, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ClassOf.GetSymbolLength() +
                ClassHelper.GetSymbolLength() +
                ClassDef.GetSymbolLength() +
                InterfaceDef.GetSymbolLength() +
                ObjectDecl.GetSymbolLength() +
                RecordDecl.GetSymbolLength() +
                RecordHelper.GetSymbolLength();

    }
}