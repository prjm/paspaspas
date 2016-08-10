﻿namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor for terminal nodes
    /// </summary>
    public class TerminalVisitor : SyntaxPartVisitorBase<TerminalVisitorOptions> {

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public override void BeginVisit(ISyntaxPart syntaxPart, TerminalVisitorOptions parameter) {
            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
        }

        /// <summary>
        ///     other tree nodes
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ISyntaxPart syntaxPart, TerminalVisitorOptions parameter) { }

        /// <summary>
        ///     get terminal string and append it to the result
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Terminal syntaxPart, TerminalVisitorOptions parameter) {

            if (syntaxPart.Token == null)
                return;

            if (syntaxPart.Token.InvalidTokens.IsValueCreated) {
                foreach (var token in syntaxPart.Token.InvalidTokens.Value)
                    parameter.ResultBuilder.Append(token.Value);
            }
            parameter.ResultBuilder.Append(syntaxPart.Token.Value);
        }

    }
}
