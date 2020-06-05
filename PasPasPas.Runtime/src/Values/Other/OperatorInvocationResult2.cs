#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     operator invocation result
    /// </summary>
    internal class OperatorInvocationResult2 : Signature2, IOperatorInvocationResult {

        /// <summary>
        ///     create a new result
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="resultType"></param>
        /// <param name="typeDefinition1"></param>
        /// <param name="typeDefinition2"></param>
        public OperatorInvocationResult2(OperatorKind kind, ITypeSymbol resultType, ITypeSymbol typeDefinition1, ITypeSymbol typeDefinition2)
            : base(resultType, typeDefinition1, typeDefinition2)
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