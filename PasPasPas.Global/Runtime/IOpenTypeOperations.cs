namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     value factory for undefined types
    /// </summary>
    public interface IOpenTypeOperations {

        /// <summary>
        ///     produce a compile-time undetermined type value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>type value</returns>
        IValue ByTypeId(int typeId);
    }
}
