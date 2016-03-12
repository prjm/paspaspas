using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : ISyntaxPart {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">information provider</param>
        protected SyntaxPartBase(IParserInformationProvider informationProvider) {
            InformationProvider = informationProvider;
        }

        /// <summary>
        ///     information provider
        /// </summary>
        public IParserInformationProvider InformationProvider { get; }

        /// <summary>
        ///     output syntax part to a formatter
        /// </summary>
        /// <param name="result">formatter</param>
        public abstract void ToFormatter(PascalFormatter result);
    }
}