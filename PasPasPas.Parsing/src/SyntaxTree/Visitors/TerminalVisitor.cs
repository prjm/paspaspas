#nullable disable
using System.Text;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor for terminal nodes
    /// </summary>
    public class TerminalVisitor : IStartVisitor<Terminal> {

        private readonly Visitor visitor;

        /// <summary>
        ///     get non-generic visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        /// <summary>
        ///     creates a new visitor
        /// </summary>
        public TerminalVisitor()
            => visitor = new Visitor(this);

        /// <summary>
        ///     get terminal string and append it to the result
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(Terminal element) {
            if (element.Prefix != null)
                foreach (var prefix in element.Prefix)
                    ResultBuilder.Append(prefix.Value);

            ResultBuilder.Append(element.Token.Value);

            if (element.Suffix != null)
                foreach (var suffix in element.Suffix)
                    ResultBuilder.Append(suffix.Value);

        }

        /// <summary>
        ///     result builder
        /// </summary>
        public StringBuilder ResultBuilder { get; }
             = new StringBuilder();

    }
}
