namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     enumerated types
    /// </summary>
    public interface IEnumeratedType : IOrdinalType {

        /// <summary>
        ///     enumerated type definition
        /// </summary>
        ITypeDefinition CommonTypeId { get; }
    }
}
