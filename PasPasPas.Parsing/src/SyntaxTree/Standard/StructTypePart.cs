using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     structured type part symbol
    /// </summary>
    public class StructTypePart : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new structured type part
        /// </summary>
        /// <param name="arrayTypeSymbol"></param>
        public StructTypePart(ArrayTypeSymbol arrayTypeSymbol)
            => ArrayType = arrayTypeSymbol;

        /// <summary>
        ///     create a new structured type part
        /// </summary>
        /// <param name="setDefinition"></param>
        public StructTypePart(SetDefinitionSymbol setDefinition)
            => SetType = setDefinition;

        /// <summary>
        ///     create new structured type part
        /// </summary>
        /// <param name="fileTypeSymbol"></param>
        public StructTypePart(FileTypeSymbol fileTypeSymbol)
            => FileType = fileTypeSymbol;

        /// <summary>
        ///     create a new structured type part
        /// </summary>
        /// <param name="classTypeDeclarationSymbol"></param>
        public StructTypePart(ClassTypeDeclarationSymbol classTypeDeclarationSymbol)
            => ClassDeclaration = classTypeDeclarationSymbol;

        /// <summary>
        ///     array type
        /// </summary>
        public ArrayTypeSymbol ArrayType { get; }

        /// <summary>
        ///     class type declaration
        /// </summary>
        public ClassTypeDeclarationSymbol ClassDeclaration { get; }

        /// <summary>
        ///     file type declaration
        /// </summary>
        public FileTypeSymbol FileType { get; }

        /// <summary>
        ///     set type declaration
        /// </summary>
        public SetDefinitionSymbol SetType { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ArrayType, visitor);
            AcceptPart(this, SetType, visitor);
            AcceptPart(this, FileType, visitor);
            AcceptPart(this, ClassDeclaration, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ArrayType.GetSymbolLength() +
                SetType.GetSymbolLength() +
                FileType.GetSymbolLength() +
                ClassDeclaration.GetSymbolLength();

    }
}