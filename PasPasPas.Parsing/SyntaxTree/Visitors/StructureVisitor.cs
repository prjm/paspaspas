namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     structure visitor
    /// </summary>
    public class StructureVisitor : SyntaxPartVisitorBase<StructureVisitorOptions> {

        public override bool BeginVisit(ISyntaxPart syntaxPart, StructureVisitorOptions parameter) {
            base.BeginVisit(syntaxPart, parameter);
            parameter.ResultBuilder.Append(new string(' ', 2 * parameter.Indent));
            parameter.ResultBuilder.Append(syntaxPart.GetType().Name);
            parameter.Indent++;

            if (syntaxPart is Terminal) {
                parameter.ResultBuilder.Append(" '");
                parameter.ResultBuilder.Append(((Terminal)syntaxPart).Token.Value);
                parameter.ResultBuilder.Append("'");
            }

            parameter.ResultBuilder.AppendLine();

            return true;
        }

        public override bool EndVisit(ISyntaxPart syntaxPart, StructureVisitorOptions parameter) {
            parameter.Indent--;
            return base.EndVisit(syntaxPart, parameter);
        }

    }
}
