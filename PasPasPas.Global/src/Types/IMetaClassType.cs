namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     meta class type
    /// </summary>
    public interface IMetaClassType : ITypeDefinition {

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseTypeId { get; }
    }
}
