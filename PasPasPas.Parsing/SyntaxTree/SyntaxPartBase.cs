using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : ISyntaxPart, IFormattableSyntaxPart {

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
        ///     accept this visitor
        /// </summary>
        /// <param name="visitor"></param>
        public virtual void Accept(ISyntaxPartVisitor visitor) {
            visitor.BeginVisit(this);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     output syntax part to a formatter
        /// </summary>                                      
        /// <param name="result">formatter</param>
        public abstract void ToFormatter(PascalFormatter result);
    }
}