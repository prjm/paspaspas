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
        /// <param name="parameter"></param>
        public void StartVisit(Terminal syntaxPart)
            => ResultBuilder.Append(syntaxPart.Token.Value);

        /*
        foreach (Token token in syntaxPart.Token.InvalidTokensBefore)
            ResultBuilder.Append(token.Value);
            */

        /*
        foreach (Token token in syntaxPart.Token.InvalidTokensAfter)
                ResultBuilder.Append(token.Value);
            */

        /// <summary>
        ///     result builder
        /// </summary>
        public StringBuilder ResultBuilder { get; }
         = new StringBuilder();

    }
}
