using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     generic type ident
    /// </summary>
    public class GenericTypeIdent : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public GenericTypeIdent(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; internal set; }

        /// <summary>
        ///     type name
        /// </summary>
        public PascalIdentifier Ident { get; internal set; }

        /// <summary>
        ///     format type declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            Ident.ToFormatter(result);
            if (GenericDefinition != null) {
                GenericDefinition.ToFormatter(result);
            }
        }
    }
}