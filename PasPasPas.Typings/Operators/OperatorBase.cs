using PasPasPas.Global.Constants;
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
        ///     operator kind
        /// </summary>
        /// <see cref="DefinedOperators"/>
        public int Kind
            => kind;

        /// <summary>
        ///     operator arity (number of operands)
        /// </summary>
        public int Arity
            => arity;

        /// <summary>
        ///     operator name (optional)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     type registry for type operation
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
        public IValue EvaluateOperator(Signature input) {
            switch (arity) {
                case 1:
                    return Runtime.Indetermined.ByTypeId(EvaluateUnaryOperator(input));
                case 2:
                    return Runtime.Indetermined.ByTypeId(EvaluateBinaryOperator(input));
            }

            return Runtime.Indetermined.ByTypeId(KnownTypeIds.ErrorType);
        }

        /// <summary>
        ///     evaluate unary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual int EvaluateBinaryOperator(Signature input)
            => KnownTypeIds.ErrorType;

        /// <summary>
        ///     evaluate binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual int EvaluateUnaryOperator(Signature input)
            => KnownTypeIds.ErrorType;

        /// <summary>
        ///     compute the operator value
        /// </summary>
        /// <param name="inputs">constant inputs</param>
        /// <returns>operator valueB</returns>
        public abstract IValue ComputeValue(IValue[] inputs);
    }
}