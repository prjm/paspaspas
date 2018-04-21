namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     type reference
    /// </summary>
    public interface ITypeReference {

        /// <summary>
        ///     get the well-known type id of this value
        /// </summary>
        /// <see cref="Constants.KnownTypeIds"/>
        int TypeId { get; }


    }
}