using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     operator result
    /// </summary>
    internal class OperatorInvocationResult1 : Signature1, IOperatorInvocationResult {

        /// <summary>
        ///     create a new operator invocation result
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="returnType"></param>
        /// <param name="parameter"></param>
        public OperatorInvocationResult1(OperatorKind kind, ITypeSymbol returnType, ITypeSymbol parameter) : base(returnType, parameter)
            => Kind = kind;

        /// <summary>
        ///     operator kind
        /// </summary>
        public OperatorKind Kind { get; }

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => ReturnType.TypeDefinition;

        /// <summary>
        ///     operator result
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.OperatorResult;
    }
}
