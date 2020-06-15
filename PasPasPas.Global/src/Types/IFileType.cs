namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     file type definition
    /// </summary>
    public interface IFileType : ITypeDefinition {

        /// <summary>
        ///     base type definition
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }
    }
}
