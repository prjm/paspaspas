using System.Text;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor for terminal nodes
    /// </summary>
    public class TerminalVisitor :
        IStartVisitor<Terminal> {

        private readonly Visitor visitor;

        /// <summary>
        ///     get nongeneric visitor
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
        /// <param name="syntaxPart"></param>
        public void StartVisit(Terminal syntaxPart) {
            if (syntaxPart.Prefix != null)
                foreach (var prefix in syntaxPart.Prefix)
                    ResultBuilder.Append(prefix.Value);

            ResultBuilder.Append(syntaxPart.Token.Value);

            if (syntaxPart.Suffix != null)
                foreach (var suffix in syntaxPart.Suffix)
                    ResultBuilder.Append(suffix.Value);

        }

        /// <summary>
        ///     result builder
        /// </summary>
        public StringBuilder ResultBuilder { get; }
             = new StringBuilder();

    }
}
