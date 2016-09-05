using System.Collections.Generic;
using PasPasPas.Api;
using System;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : SyntraxTreeNodeBase, IFormattableSyntaxPart {

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
        ///     information provider
        /// </summary>
        public IParserInformationProvider InformationProvider { get; }

        /// <summary>
        ///     syntax tree parts
        /// </summary>
        public override IEnumerable<ISyntaxTreeNode> Parts
        {
            get
            {
                if (parts.IsValueCreated) {
                    foreach (var part in parts.Value) {
                        yield return part;
                    }
                }
            }
        }

        private Lazy<IList<ISyntaxTreeNode>> parts
            = new Lazy<IList<ISyntaxTreeNode>>(() => new List<ISyntaxTreeNode>());

        /// <summary>
        ///     get the last terminal symbol
        /// </summary>
        public Terminal LastTerminal
        {
            get
            {
                if (parts.IsValueCreated && parts.Value.Count > 0)
                    return parts.Value[parts.Value.Count - 1] as Terminal;
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
        public static IEnumerable<Terminal> FindAllTerminals(ISyntaxTreeNode symbol) {
            if (symbol is Terminal)
                yield return symbol as Terminal;

            foreach (var child in symbol.Parts)
                FindAllTerminals(child);
        }
    }
}