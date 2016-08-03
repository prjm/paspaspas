using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     object item
    /// </summary>
    public class ObjectItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ObjectItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassField FieldDeclaration { get; internal set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; internal set; }

        /// <summary>
        ///     strict modifier
        /// </summary>
        public bool Strict { get; internal set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; internal set; }

        /// <summary>
        ///     format objct item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (MethodDeclaration != null) {
                result.Part(MethodDeclaration);
                return;
            }

            if (FieldDeclaration != null) {
                result.Part(FieldDeclaration);
                return;
            }

            if (Visibility != PascalToken.Undefined) {
                ClassDeclarationItem.FormatVisibility(result, Visibility, Strict);
            }
        }
    }
}
