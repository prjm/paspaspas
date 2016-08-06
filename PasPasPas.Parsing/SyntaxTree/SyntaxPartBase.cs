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
        [System.Obsolete()]
        protected SyntaxPartBase(IParserInformationProvider informationProvider) {
            InformationProvider = informationProvider;
        }

        /// <summary>
        ///     information provider
        /// </summary>
        public IParserInformationProvider InformationProvider { get; }

        /// <summary>
        ///     syntax par
        /// </summary>
        public virtual ICollection<ISyntaxPart> Parts { get; }
            = new List<ISyntaxPart>();

        /// <summary>
        ///     accept this visitor
        /// </summary>
        /// <param name="visitor"></param>
        public virtual void Accept<TParam>(ISyntaxPartVisitor<TParam> visitor, TParam param) {
            visitor.BeginVisit(this, param);

            foreach (var part in Parts)
                part.Accept(visitor, param);

            visitor.EndVisit(this, param);
        }

        /// <summary>
        ///     output syntax part to a formatter
        /// </summary>                                      
        /// <param name="result">formatter</param>
        public virtual void ToFormatter(PascalFormatter result) {
            //..
        }
    }
}