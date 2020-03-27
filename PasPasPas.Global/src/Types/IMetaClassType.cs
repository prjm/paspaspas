namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     meta class type
    /// </summary>
    public interface IMetaType : ITypeDefinition {

        /// <summary>
        ///     base type id
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }
    }
}
