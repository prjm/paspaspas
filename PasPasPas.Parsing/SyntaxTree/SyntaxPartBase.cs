using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : ISyntaxPart {

        /// <summary>
        ///     create a new syntax part base
        /// </summary>
        protected SyntaxPartBase() {
            //..
        }

        /// <summary>
        ///     parent node
        /// </summary>
        public ISyntaxPart Parent { get; set; }

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
        /// <param name="visitorParameter">parameter</param>
        /// <typeparam name="T">parameter type</typeparam>
        public virtual bool Accept<T>(ISyntaxPartVisitor<T> visitor, T visitorParameter) {
            if (!visitor.BeginVisit(this, visitorParameter))
                return false;

            var result = true;

            foreach (var part in Parts)
                result = result && part.Accept(visitor, visitorParameter);

            if (!visitor.EndVisit(this, visitorParameter))
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