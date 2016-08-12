using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor to interpret compiler directives
    /// </summary>
    public class CompilerDirectiveVisitor : SyntaxPartVisitorBase<CompilerDirectiveVisitorOptions> {

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public override void BeginVisit(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
        }

        /// <summary>
        ///     other tree nodes
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) { }

        /// <summary>
        ///     update alignment
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(AlignSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Align.Value = syntaxPart.AlignValue;
        }

        /// <summary>
        ///     update application type
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(AppTypeParameter syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.ApplicationType.Value = syntaxPart.ApplicationType;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(AssertSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.Assertions.Value = syntaxPart.Assertions;
        }

        /// <summary>
        ///     update assertion mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(BooleanEvaluationSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.BoolEval.Value = syntaxPart.BoolEval;
        }

        /// <summary>
        ///     update code align
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(CodeAlignParameter syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.CodeAlign.Value = syntaxPart.CodeAlign;
        }

        /// <summary>
        ///     update debug info mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(DebugInfoSwitch syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.CompilerOptions.DebugInfo.Value = syntaxPart.DebugInfo;
        }

        /// <summary>
        ///     update debug info mode
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(DefineSymbol syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.DefineSymbol(syntaxPart.SymbolName);
        }

    }
}
