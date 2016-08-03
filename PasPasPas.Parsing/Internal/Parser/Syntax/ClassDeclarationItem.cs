using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     class declaration item
    /// </summary>
    public class ClassDeclarationItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassDeclarationItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     class-wide declaration
        /// </summary>
        public bool Class { get; internal set; }

        /// <summary>
        ///     constant class section
        /// </summary>
        public ConstSection ConstSection { get; internal set; }

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassField FieldDeclaration { get; internal set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; internal set; }

        /// <summary>
        ///     method resolution
        /// </summary>
        public MethodResolution MethodResolution { get; internal set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty PropertyDeclaration { get; internal set; }

        /// <summary>
        ///     strict declaration
        /// </summary>
        public bool Strict { get; internal set; }

        /// <summary>
        ///     type section
        /// </summary>
        public TypeSection TypeSection { get; internal set; }

        /// <summary>
        ///     variabkes
        /// </summary>
        public VarSection VarSection { get; internal set; }

        /// <summary>
        ///     visibility declaration
        /// </summary>
        public int Visibility { get; internal set; }
            = PascalToken.Undefined;

        /// <summary>
        ///     format source
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Attributes).NewLine();

            if (MethodResolution != null) {
                if (Class) {
                    result.Keyword("class");
                    result.Space();
                }
                result.Part(MethodResolution);
                return;
            }

            if (MethodDeclaration != null) {
                if (Class) {
                    result.Keyword("class");
                    result.Space();
                }
                result.Part(MethodDeclaration);
                return;
            }

            if (FieldDeclaration != null) {
                result.Part(FieldDeclaration);
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

            if (ConstSection != null) {
                ConstSection.ToFormatter(result);
                return;
            }

            if (TypeSection != null) {
                TypeSection.ToFormatter(result);
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
                FormatVisibility(result, Visibility, Strict);
            }
        }

        internal static void FormatVisibility(PascalFormatter result, int visibility, bool strict) {
            if (strict) {
                result.Keyword("strict");
                result.Space();
            };

            switch (visibility) {
                case PascalToken.Private:
                    result.Keyword("private");
                    break;

                case PascalToken.Protected:
                    result.Keyword("protected");
                    break;

                case PascalToken.Public:
                    result.Keyword("public");
                    break;

                case PascalToken.Published:
                    result.Keyword("published");
                    break;

                case PascalToken.Automated:
                    result.Keyword("automated");
                    break;
            }

            result.Space();
        }
    }
}