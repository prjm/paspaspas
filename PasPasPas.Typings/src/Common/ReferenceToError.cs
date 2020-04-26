using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     reference to the error type
    /// </summary>
    internal class ReferenceToError : ITypeSymbol {

        /// <summary>
        ///     create a new reference to the error type
        /// </summary>
        /// <param name="errorType"></param>
        internal ReferenceToError(ITypeDefinition errorType)
            => ErrorType = errorType;

        /// <summary>
        ///     error type
        /// </summary>
        public ITypeDefinition ErrorType { get; }

        /// <summary>
        ///     error type
        /// </summary>
        public ITypeDefinition TypeDefinition
            => ErrorType;

        /// <summary>
        ///     undefined symbol kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.Undefined;
    }
}