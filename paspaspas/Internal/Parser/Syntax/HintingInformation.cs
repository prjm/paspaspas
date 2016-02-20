using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     additinal hinting information
    /// </summary>
    public class HintingInformation : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public HintingInformation(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     hint for deprecation
        /// </summary>
        public bool Deprecated { get; internal set; }

        /// <summary>
        ///     comment for deprecation
        /// </summary>
        public QuotedString DeprecatedComment { get; internal set; }

        /// <summary>
        ///     hint for experimental
        /// </summary>
        public bool Experimental { get; internal set; }

        /// <summary>
        ///     hint for library
        /// </summary>
        public bool Library { get; internal set; }

        /// <summary>
        ///     hint for platform
        /// </summary>
        public bool Platform { get; internal set; }

        /// <summary>
        ///     format hinting directive
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            if (Deprecated) {
                result.Space();
                result.Keyword("deprecated");

                if (DeprecatedComment != null) {
                    result.Space();
                    result.Part(DeprecatedComment);
                }
            }

            if (Experimental) {
                result.Space();
                result.Keyword("experimental");
            }

            if (Platform) {
                result.Space();
                result.Keyword("platform");
            }

            if (Library) {
                result.Space();
                result.Keyword("library");
            }
        }
    }
}