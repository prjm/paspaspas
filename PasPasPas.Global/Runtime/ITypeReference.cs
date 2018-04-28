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

        /// <summary>
        ///     if <c>true</c> this value represents a compile-time constant typed value
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        CommonTypeKind TypeKind { get; }

    }
}