﻿namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for expression parts
    /// </summary>
    public abstract class ExpressionBase : AbstractSyntaxPartBase, IExpression {

        /// <summary>
        ///     calculated type value
        /// </summary>
        public ITypeDefinition TypeInfo { get; set; }

    }
}
