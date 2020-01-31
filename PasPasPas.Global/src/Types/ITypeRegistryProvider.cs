namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     type registry provider
    /// </summary>
    public interface ITypeRegistryProvider {

        /// <summary>
        ///     type registry
        /// </summary>
        ITypeRegistry RegisteredTypes { get; }

    }

    /// <summary>
    ///     type registry provider helpers
    /// </summary>
    public static class TypeRegistryProviderHelper {

        /// <summary>
        ///     resolve the boolean type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IBooleanType GetBooleanType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.BooleanType;

        /// <summary>
        ///     resolve the boolean type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ICharType GetWideCharType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.WideCharType;


        /// <summary>
        ///     resolve the UNICODE string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetUnicodeStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.UnicodeStringType;

        /// <summary>
        ///     ANSI string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetAnsiStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.AnsiStringType;

        /// <summary>
        ///     short string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetShortStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ShortStringType;

        /// <summary>
        ///     extended floating point precision type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IRealType GetExtendedType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ExtendedType;

    }

}
