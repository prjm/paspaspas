using PasPasPas.Globals.Runtime;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for expression parts
    /// </summary>
    public abstract class ExpressionBase : AbstractSyntaxPartBase, IExpression {

        /// <summary>
        ///     type value of this expression
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

    }
}
