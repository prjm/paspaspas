namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     character type
    /// </summary>
    public interface ICharType : IOrdinalType {

        /// <summary>
        ///     char type kind
        /// </summary>
        CharTypeKind Kind { get; }

    }
}
