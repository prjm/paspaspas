using PasPasPas.Parsing.SyntaxTree.Standard;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : IExtendableSyntaxPart {

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
        public virtual IEnumerable<ISyntaxPart> Parts
            => parts;

        private List<ISyntaxPart> parts
            = new List<ISyntaxPart>();

        /// <summary>
        ///     list of parts
        /// </summary>
        public IList<ISyntaxPart> PartList
            => parts;

        /// <summary>
        ///     get the last terminal value
        /// </summary>
        public string LastTerminalValue
        {
            get
            {
                if (parts.Count < 1)
                    return string.Empty;


                var terminal = parts[parts.Count - 1] as Terminal;

                if (terminal == null)
                    return string.Empty;

                return terminal.Value;
            }
        }

        /// <summary>
        ///     get the last terminal symbol
        /// </summary>
        public int LastTerminalKind
        {
            get
            {
                if (parts.Count < 1)
                    return TokenKind.Undefined;


                var terminal = parts[parts.Count - 1] as Terminal;

                if (terminal == null)
                    return TokenKind.Undefined;

                return terminal.Kind;
            }
        }

        /// <summary>
        ///     first terminal
        /// </summary>
        public Token FirstTerminalToken
        {
            get
            {
                if (parts.Count < 1)
                    return null;


                var terminal = parts[0] as Terminal;

                if (terminal == null)
                    return null;

                return terminal.Token;
            }
        }

        /// <summary>
        ///     get the last terminal symbol
        /// </summary>
        public Token LastTerminalToken
        {
            get
            {
                if (parts.Count < 1)
                    return null;


                var terminal = parts[parts.Count - 1] as Terminal;

                if (terminal == null)
                    return null;

                return terminal.Token;
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

        /// <summary>
        ///     add an iten
        /// </summary>
        /// <param name="result"></param>
        public void Add(ISyntaxPart result) {
            parts.Add(result);
        }

        /// <summary>
        ///     remove an item
        /// </summary>
        /// <param name="lastSymbol"></param>
        public void Remove(ISyntaxPart lastSymbol) {
            parts.Remove(lastSymbol);
        }

        /// <summary>
        ///     value of a terminal
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <returns></returns>
        public static string TerminalValue(ISyntaxPart syntaxPart) {
            var result = syntaxPart as Terminal;
            if (result != null)
                return result.Token.Value;
            else
                return null;
        }

        /// <summary>
        ///     value of an identifier
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static string IdentifierValue(ISyntaxPart part) {
            var result = part as Identifier;
            if (result != null && result.parts.Count > 0)
                return TerminalValue(result.parts[0]);
            else
                return null;
        }
    }
}