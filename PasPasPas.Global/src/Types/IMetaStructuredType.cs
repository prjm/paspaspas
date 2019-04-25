namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     create a meta structured type
    /// </summary>
    public interface IMetaStructuredType : ITypeDefinition, IExtensibleGenericType {

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseType { get; }

    }
}
