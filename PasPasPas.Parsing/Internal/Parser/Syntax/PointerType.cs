using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     pointer type specification
    /// </summary>
    public class PointerType : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public PointerType(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     true if a generic pointer type is found
        /// </summary>
        public bool GenericPointer { get; internal set; }
            = false;

        /// <summary>
        ///     type specification for non generic pointers
        /// </summary>
        public TypeSpecification TypeSpecification { get; internal set; }

        /// <summary>
        ///     format pointer type
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            if (GenericPointer) {
                result.Keyword("Pointer");
                return;
            }

            TypeSpecification.ToFormatter(result);
        }
    }
}