using System.Diagnostics;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntax pars
    /// </summary>
    public abstract class SyntaxPartBase : ISyntaxPart {

        /// <summary>
        ///     create a new syntax part base
        /// </summary>
        protected SyntaxPartBase()
            => LogHistogram();

        [Conditional("DEBUG")]
        private void LogHistogram() {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.SyntaxNodes, GetType().Name);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public virtual int Length { get; }

        /// <summary>
        ///     value of a terminal
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <returns></returns>
        public static string TerminalValue(ISyntaxPart syntaxPart) {
            if (syntaxPart is Terminal result)
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
            if (part is IdentifierSymbol result && result.Symbol != default)
                return TerminalValue(result.Symbol);
            else
                return null;
        }

        /// <summary>
        ///     visit a child node
        /// </summary>
        /// <typeparam name="T">visitor type</typeparam>
        /// <param name="element"></param>
        /// <param name="part"></param>
        /// <param name="visitor"></param>
        protected static void AcceptPart<T>(T element, ISyntaxPart part, IStartEndVisitor visitor) {

            if (part == null || part is Terminal t && t.Kind == TokenKind.Empty)
                return;

            var childVisitor = visitor as IChildVisitor;
            childVisitor?.StartVisitChild<T>(element, part);
            part.Accept(visitor);
            childVisitor?.EndVisitChild<T>(element, part);
        }

        /// <summary>
        ///     accept visitors
        /// </summary>
        public abstract void Accept(IStartEndVisitor visitor);

    }
}