namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     structure visitor
    /// </summary>
    public class StructureVisitor : SyntaxPartVisitorBase<StructureVisitorOptions> {

        /// <summary>
        ///     start visiting structure
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool BeginVisit(ISyntaxPart syntaxPart, StructureVisitorOptions parameter) {
            base.BeginVisit(syntaxPart, parameter);
            parameter.ResultBuilder.Append(new string(' ', 2 * parameter.Indent));
            parameter.ResultBuilder.Append(syntaxPart.GetType().Name);
            parameter.Indent++;

            var terminal = syntaxPart as Terminal;
            if (terminal != null) {
                parameter.ResultBuilder.Append(" '");
                parameter.ResultBuilder.Append(terminal.Token.Value);
                parameter.ResultBuilder.Append("'");
            }

            parameter.ResultBuilder.AppendLine();

            return true;
        }

        /// <summary>
        ///     end visitg
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool EndVisit(ISyntaxPart syntaxPart, StructureVisitorOptions parameter) {
            parameter.Indent--;
            return base.EndVisit(syntaxPart, parameter);
        }

    }
}
