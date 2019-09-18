namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a unit type definition
    /// </summary>
    public interface IUnitType : ITypeDefinition {

        /// <summary>
        ///     unit name
        /// </summary>
        string Name { get; }
    }
}
