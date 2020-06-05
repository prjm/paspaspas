#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     result of a dynamic cast
    /// </summary>
    internal class CastResult : ITypeSymbol {

        /// <summary>
        ///     create a new cast result
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        internal CastResult(ITypeSymbol fromType, ITypeDefinition toType) {
            FromType = fromType;
            ToType = toType;
        }

        /// <summary>
        ///     source type
        /// </summary>
        public ITypeSymbol FromType { get; }

        /// <summary>
        ///     target type
        /// </summary>
        public ITypeDefinition ToType { get; }

        /// <summary>
        ///     target type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => ToType;

        /// <summary>
        ///     cast result
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.CastResult;
    }
}