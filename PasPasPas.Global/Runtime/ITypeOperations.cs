namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     value factory for undefined types
    /// </summary>
    public interface ITypeOperations {

        /// <summary>
        ///     produces a reference to a type with indeterminate compile-time value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>reference to type</returns>
        ITypeReference MakeReference(int typeId);
    }
}
