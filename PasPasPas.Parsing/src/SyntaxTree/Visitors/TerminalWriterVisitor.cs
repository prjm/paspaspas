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
        /// <param name="element"></param>
        public void StartVisit(Terminal element) {
            if (element.Prefix != null)
                foreach (var prefix in element.Prefix)
                    ResultBuilder.Write(prefix.Value);

            ResultBuilder.Write(element.Token.Value);

            if (element.Suffix != null)
                foreach (var suffix in element.Suffix)
                    ResultBuilder.Write(suffix.Value);

        }

        /// <summary>
        ///     output
        /// </summary>
        public TextWriter ResultBuilder { get; }


    }
}
