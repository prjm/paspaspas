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
        /// <param name="provder"></param>
        /// <returns></returns>
        public static ITypeDefinition GetBooleanType(this ITypeRegistryProvider provder)
            => provder.RegisteredTypes.SystemUnit.BooleanType;

    }

}
