using System;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor to interpret compiler directives
    /// </summary>
    public class CompilerDirectiveVisitor : SyntaxPartVisitorBase<CompilerDirectiveVisitorOptions> {

        /// <summary>
        ///     test if an item can be visited
        /// </summary>
        /// <param name="syntaxPart">syntax part to test</param>
        /// <param name="parameter">options</param>
        /// <returns></returns>
        private bool CanVisit(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (!parameter.ConditionalCompilation.Skip)
                return true;

            return syntaxPart is EndIf || syntaxPart is IfDef || syntaxPart is Else;
        }

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public override bool BeginVisit(ISyntaxPart syntaxPart, CompilerDirectiveVisitorOptions parameter) {

            if (!CanVisit(syntaxPart, parameter))
                return true;

            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
            return true;
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
        ///     define symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(DefineSymbol syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.DefineSymbol(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     undefine symbol
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(UnDefineSymbol syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.UndefineSymbol(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("ifdef")
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(IfDef syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            if (syntaxPart.Negate)
                parameter.ConditionalCompilation.AddIfNDefCondition(syntaxPart.SymbolName);
            else
                parameter.ConditionalCompilation.AddIfDefCondition(syntaxPart.SymbolName);
        }

        /// <summary>
        ///     conditional compilation ("ifdef")
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(EndIf syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.RemoveIfDefCondition();
        }


        /// <summary>
        ///     conditional compilation ("else")
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        public void BeginVisitItem(Else syntaxPart, CompilerDirectiveVisitorOptions parameter) {
            parameter.ConditionalCompilation.AddElseCondition();
        }

    }
}
