namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     value factory for undefined types
    /// </summary>
    public interface ITypeOperations {

        /// <summary>
        ///     constant nil pointer
        /// </summary>
        IValue Nil { get; }

        /// <summary>
        ///     type resolver
        /// </summary>
        ITypeKindResolver Resolver { get; }

        /// <summary>
        ///     produces a reference to a type with indeterminate compile-time value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>reference to type</returns>
        ITypeReference MakeReference(int typeId);

        /// <summary>
        ///     make an enumerated type value
        /// </summary>
        /// <param name="enumTypeId">type id of the enumerated type</param>
        /// <param name="value">constant value</param>
        /// <returns>enumerated type value</returns>
        ITypeReference MakeEnumValue(int enumTypeId, ITypeReference value);
    }
}
