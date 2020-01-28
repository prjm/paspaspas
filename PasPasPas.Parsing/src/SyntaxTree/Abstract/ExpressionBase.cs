using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for expression parts
    /// </summary>
    public abstract class ExpressionBase : AbstractSyntaxPartBase, IExpression {

        /// <summary>
        ///     type value of this expression
        /// </summary>
        public ITypeSymbol TypeInfo { get; set; }

    }
}
