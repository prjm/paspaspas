#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for type aliases
    /// </summary>
    public interface IAliasedType : ITypeDefinition {

        /// <summary>
        ///     base type (aliased type)
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     <c>true</c> if this is a new type definition
        /// </summary>
        bool IsNewType { get; }
    }
}
