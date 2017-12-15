using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     base clas for operators
    /// </summary>
    public abstract class OperatorBase : IOperator {

        private int kind;

        /// <summary>
        ///     create a new opertor
        /// </summary>
        /// <param name="withKind">operation kind</param>
        public OperatorBase(int withKind)
            => kind = withKind;

        /// <summary>
        ///     resolve type aliases
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        protected ITypeDefinition ResolveAlias(int typeId) {
            var type = TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
            return TypeBase.ResolveAlias(type);
        }

        /// <summary>
        ///     operation kind - <c>DefinedOperations.NotOperation</c>
        /// </summary>
        public int Kind
            => kind;

        /// <summary>
        ///     operator name (optional)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     get the output type for an operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type id</returns>
        /// <param name="currentValue">current values of the operands (if constant)</param>
        public abstract int GetOutputTypeForOperation(Signature input, object[] currentValue);
    }
}