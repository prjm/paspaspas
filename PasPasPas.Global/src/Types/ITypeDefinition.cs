namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a type definition
    /// </summary>
    public interface ITypeDefinition : ITypeSymbol {

        /// <summary>
        ///     get base type of this type definition
        /// </summary>
        BaseType BaseType { get; }

        /// <summary>
        ///     defining unit
        /// </summary>
        IUnitType DefiningUnit { get; }

        /// <summary>
        ///     type name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        uint TypeSizeInBytes { get; }

        /// <summary>
        ///     mangled type name
        /// </summary>
        string MangledName { get; }

    }
}