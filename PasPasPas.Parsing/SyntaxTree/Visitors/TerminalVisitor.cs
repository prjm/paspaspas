using System.Text;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor for terminal nodes
    /// </summary>
    public class TerminalVisitor :
        IStartVisitor<Terminal> {

        private readonly Visitor visitor;

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
        /// <param name="syntaxPart"></param>
        public void StartVisit(Terminal syntaxPart) {
            ResultBuilder.Append(syntaxPart.Prefix);
            ResultBuilder.Append(syntaxPart.Token.Value);
            ResultBuilder.Append(syntaxPart.Suffix);
        }

        /// <summary>
        ///     result builder
        /// </summary>
        public StringBuilder ResultBuilder { get; }
             = new StringBuilder();

    }
}
