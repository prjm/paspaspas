using PasPasPas.Global.Runtime;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for expression parts
    /// </summary>
    public abstract class ExpressionBase : AbstractSyntaxPartBase, IExpression {

        /// <summary>
        ///     calculated type value
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

        /// <summary>
        ///     test if this expression value is constant at compile time
        /// </summary>
        public bool IsConstant
            => TypeInfo is IValue;


    }
}
