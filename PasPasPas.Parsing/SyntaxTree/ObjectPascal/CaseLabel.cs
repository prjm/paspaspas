﻿namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     case label
    /// </summary>
    public class CaseLabel : SyntaxPartBase {

        /// <summary>
        ///     end expression
        /// </summary>
        public Expression EndExpression { get; set; }

        /// <summary>
        ///     start expression
        /// </summary>
        public Expression StartExpression { get; set; }

    }
}