namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     helper interface: get type kinds of type ids
    /// </summary>
    public interface ITypeKindResolver {

        /// <summary>
        ///     get the type kind of a type id
        /// </summary>
        /// <param name="typeId">given type id</param>
        /// <returns>type kind</returns>
        CommonTypeKind GetTypeKindOf(int typeId);

        /// <summary>
        ///     get the base type of a subrange type
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>base type</returns>
        int GetBaseTypeOfSubrangeType(int typeId);

    }
}
