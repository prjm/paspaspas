using System.IO;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor to write terminals to a stream
    /// </summary>
    public class TerminalWriterVisitor : IStartVisitor<Terminal> {

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
        public TerminalWriterVisitor(TextWriter result) {
            visitor = new Visitor(this);
            ResultBuilder = result;
        }

        /// <summary>
        ///     get terminal string and append it to the result
        /// </summary>
        /// <param name="syntaxPart"></param>
        public void StartVisit(Terminal syntaxPart) {
            if (syntaxPart.Prefix != null)
                foreach (var prefix in syntaxPart.Prefix)
                    ResultBuilder.Write(prefix.Value);

            ResultBuilder.Write(syntaxPart.Token.Value);

            if (syntaxPart.Suffix != null)
                foreach (var suffix in syntaxPart.Suffix)
                    ResultBuilder.Write(suffix.Value);

        }

        /// <summary>
        ///     output
        /// </summary>
        public TextWriter ResultBuilder { get; }


    }
}
