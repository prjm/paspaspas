﻿using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;

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


    }
}
