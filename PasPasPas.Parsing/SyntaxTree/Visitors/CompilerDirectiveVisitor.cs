using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor to interpret compiler directives
    /// </summary>
    public class CompilerDirectiveVisitor : SyntaxPartVisitorBase<CompilerDirectiveVisitorOptions> {

        /// <summary>
        ///     update aling
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisit(AlignSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.Environemnt.Options.CompilerOptions.Align.Value = syntaxPart.AlignValue;
        }


    }
}
