namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for boolean types
    /// </summary>
    public interface IBooleanType : IOrdinalType {

        /// <summary>
        ///     boolean type kind
        /// </summary>
        BooleanTypeKind Kind { get; }

    }
}
