using PasPasPas.Parsing.SyntaxTree.Standard;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : IExtendableSyntaxPart {

        /// <summary>
        ///     create a new syntax part base
        /// </summary>
        protected SyntaxPartBase() =>
            parts = new SyntaxPartCollection<ISyntaxPart>(this);

        /// <summary>
        ///     parent node
        /// </summary>
        public ISyntaxPart ParentItem { get; set; }

        /// <summary>
        ///     syntax parts
        /// </summary>
        public virtual IEnumerable<ISyntaxPart> Parts
            => parts;

        private readonly ISyntaxPartList<ISyntaxPart> parts;

        /// <summary>
        ///     get the list of syntax parts
        /// </summary>
        public ISyntaxPartList<ISyntaxPart> PartList
            => parts;

        /// <summary>
        ///     get the last terminal value
        /// </summary>
        public string LastTerminalValue {
            get {
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
        public int LastTerminalKind {
            get {
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
        public Token FirstTerminalToken {
            get {
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
        public Token LastTerminalToken {
            get {
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

            foreach (ISyntaxPart child in symbol.Parts)
                FindAllTerminals(child);
        }

        /// <summary>
        ///     add an iten
        /// </summary>
        /// <param name="newChildItem"></param>
        public void Add(ISyntaxPart newChildItem)
            => parts.Add(newChildItem);

        /// <summary>
        ///     remove an item
        /// </summary>
        /// <param name="lastSymbol"></param>
        public void Remove(ISyntaxPart lastSymbol)
            => parts.Remove(lastSymbol);

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

        protected void AcceptParts<T>(T element, IStartVisitor startVisitor, IEndVisitor endVisitor) where T : class
            => AcceptParts<T>(element, Parts, startVisitor, endVisitor);

        public static void AcceptParts<T>(T element, IEnumerable<ISyntaxPart> parts, IStartVisitor startVisitor, IEndVisitor endVisitor) where T : class {
            var childVisitor = startVisitor as IChildVisitor;
            foreach (ISyntaxPart part in parts) {
                childVisitor?.StartVisitChild<T>(element, part);
                part.Accept(startVisitor, endVisitor);
                childVisitor?.EndVisitChild<T>(element, part);
            }
        }

        /// <summary>
        ///     accept visitors
        /// </summary>
        /// <param name="startVisitor"></param>
        /// <param name="endVisitor"></param>
        public abstract void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor);

    }
}