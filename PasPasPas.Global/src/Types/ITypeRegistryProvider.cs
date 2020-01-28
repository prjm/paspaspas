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
        public static ITypeDefinition GetBooleanType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.BooleanType;


        /// <summary>
        ///     resolve the boolean type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ITypeDefinition GetWideCharType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.WideCharType;



    }

}
