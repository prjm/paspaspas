using PasPasPas.Infrastructure.Common;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for expression parts
    /// </summary>
    public abstract class ExpressionBase : AbstractSyntaxPartBase, IExpression {

        /// <summary>
        ///     calculated type value
        /// </summary>
        public ITypeDefinition TypeInfo { get; set; }

        /// <summary>
        ///     <c>true</c> for constant expressions
        /// </summary>
        public bool IsConstant { get; set; }

        /// <summary>
        ///     constant literal value of this expression
        /// </summary>
        public IValue LiteralValue { get; set; }

    }
}
