using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     struct type part
    /// </summary>
    public class StructTypePart : SyntaxPartBase {


        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public StructTypePart(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     array type
        /// </summary>
        public ArrayType ArrayType { get; internal set; }

        /// <summary>
        ///     class type declaration
        /// </summary>
        public ClassTypeDeclaration ClassDecl { get; internal set; }

        /// <summary>
        ///     file type declaration
        /// </summary>
        public FileType FileType { get; internal set; }

        /// <summary>
        ///     set type declaration
        /// </summary>
        public SetDef SetType { get; internal set; }

        /// <summary>
        ///     format struct type part
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(ArrayType);
            result.Part(ClassDecl);
            result.Part(FileType);
            result.Part(SetType);
        }
    }
}