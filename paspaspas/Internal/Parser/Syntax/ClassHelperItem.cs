using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     class helper item
    /// </summary>
    public class ClassHelperItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassHelperItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     marker for class properties
        /// </summary>
        public bool Class { get; internal set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; internal set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty PropertyDeclaration { get; internal set; }

        /// <summary>
        ///     strict
        /// </summary>
        public bool Strict { get; internal set; }

        /// <summary>
        ///     variable section
        /// </summary>
        public VarSection VarSection { get; internal set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; internal set; }

        /// <summary>
        ///     format class helper item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Attributes).NewLine();

            if (MethodDeclaration != null) {
                if (Class) {
                    result.Keyword("class");
                    result.Space();
                }
                result.Part(MethodDeclaration);
                return;
            }

            if (PropertyDeclaration != null) {
                if (Class) {
                    result.Keyword("class");
                    result.Space();
                }
                PropertyDeclaration.ToFormatter(result);
                return;
            }

            if (VarSection != null) {
                if (Class) {
                    result.Keyword("class");
                    result.Space();
                }
                result.Part(VarSection);
                return;
            }

            if (Visibility != PascalToken.Undefined) {
                ClassDeclarationItem.FormatVisibility(result, Visibility, Strict);
            }
        }
    }
}