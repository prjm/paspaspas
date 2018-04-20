using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     base class for operators
    /// </summary>
    public abstract class OperatorBase : IOperator {

        private readonly int kind;
        private readonly int arity;

        /// <summary>
        ///     create a new operator definition
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public OperatorBase(int withKind, int withArity) {
            kind = withKind;
            arity = withArity;
        }

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
        ///     operator arity
        /// </summary>
        public int Arity
            => arity;

        /// <summary>
        ///     operator name (optional)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     runtime values
        /// </summary>
        public IRuntimeValueFactory Runtime { get; set; }

        /// <summary>
        ///     get the output type for an operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type id</returns>
        /// <param name="currentValue">current values of the operands (if constant)</param>
        public abstract int GetOutputTypeForOperation(Signature input, object[] currentValue);

        /// <summary>
        ///     compute the operator value
        /// </summary>
        /// <param name="inputs">constant inputs</param>
        /// <returns>operator valueB</returns>
        public abstract IValue ComputeValue(IValue[] inputs);
    }
}