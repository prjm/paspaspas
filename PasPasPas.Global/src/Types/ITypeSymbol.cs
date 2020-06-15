using System.Diagnostics.CodeAnalysis;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     a typed symbol
    /// </summary>
    public interface ITypeSymbol {

        /// <summary>
        ///     get the matching type definition
        /// </summary>
        ITypeDefinition TypeDefinition { get; }

        /// <summary>
        ///     symbol type kind
        /// </summary>
        SymbolTypeKind SymbolKind { get; }

    }


    /// <summary>
    ///     helper for type symbols
    /// </summary>
    public static class TypeSymbolHelper {

        /// <summary>
        ///     check if a type symbol has a numeric type
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static bool HasNumericType(this ITypeSymbol typeSymbol)
            => typeSymbol.TypeDefinition.IsNumericType();

        /// <summary>
        ///     check if a type symbol has a text type
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static bool HasTextType(this ITypeSymbol typeSymbol)
            => typeSymbol.TypeDefinition.IsTextType();

        /// <summary>
        ///     check if a type symbol has a subrange type
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <param name="subrangeType"></param>
        /// <returns></returns>
        public static bool HasSubrangeType(this ITypeSymbol typeSymbol, [NotNullWhen(returnValue: true)] out ISubrangeType? subrangeType)
            => typeSymbol.TypeDefinition.IsSubrangeType(out subrangeType);

        /// <summary>
        ///     get the base type of a type symbol
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static BaseType GetBaseType(this ITypeSymbol typeSymbol)
            => typeSymbol.TypeDefinition.BaseType;

    }

}
