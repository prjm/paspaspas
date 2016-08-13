using System.Collections.Generic;
using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : ISyntaxPart, IFormattableSyntaxPart {

        /// <summary>
        ///     create a new syntax part base
        /// </summary>
        protected SyntaxPartBase() {
            //..
        }

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">information provider</param>
        [System.Obsolete("Old constructor. Remove the calls during refactoring.")]
        protected SyntaxPartBase(IParserInformationProvider informationProvider) {
            InformationProvider = informationProvider;
        }

        /// <summary>
        ///     parent node
        /// </summary>
        public ISyntaxPart Parent { get; set; }

        /// <summary>
        ///     information provider
        /// </summary>
        public IParserInformationProvider InformationProvider { get; }

        /// <summary>
        ///     syntax parts
        /// </summary>
        public virtual IList<ISyntaxPart> Parts
            => parts;

        private IList<ISyntaxPart> parts
            = new List<ISyntaxPart>();

        /// <summary>
        ///     accept this visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        /// <param name="param">parameter</param>
        /// <typeparam name="T">parameter type</typeparam>
        public virtual bool Accept<T>(ISyntaxPartVisitor<T> visitor, T param) {
            if (!visitor.BeginVisit(this, param))
                return false;

            var result = true;

            foreach (var part in Parts)
                result = result && part.Accept(visitor, param);

            if (!visitor.EndVisit(this, param))
                return false;

            return result;
        }

        /// <summary>
        ///     get the last terminal symbol
        /// </summary>
        public Terminal LastTerminal
        {
            get
            {
                if (parts.Count > 0)
                    return parts[parts.Count - 1] as Terminal;
                else
                    return null;
            }
        }

        /// <summary>
        ///     output syntax part to a formatter
        /// </summary>                                      
        /// <param name="result">formatter</param>
        public virtual void ToFormatter(PascalFormatter result) {
            //..
        }

        /// <summary>
        ///     find all terminals in a syntax gtree
        /// </summary>
        /// <param name="symbol">symbol to search</param>
        /// <returns></returns>
        public static IEnumerable<Terminal> FindAllTerminals(ISyntaxPart symbol) {
            if (symbol is Terminal)
                yield return symbol as Terminal;

            foreach (var child in symbol.Parts)
                FindAllTerminals(child);
        }
    }
}