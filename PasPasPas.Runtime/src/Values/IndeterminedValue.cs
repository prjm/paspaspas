using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     indetermined value
    /// </summary>
    public class IndeterminedRuntimeValue : ITypeReference {

        /// <summary>
        ///     create a new indetermined runtime value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <param name="typeKind">type kind</param>
        public IndeterminedRuntimeValue(int typeId, CommonTypeKind typeKind) {
            TypeId = typeId;
            TypeKind = typeKind;
        }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     fixed type kind
        /// </summary>
        public CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.DynamicValue;

        /// <summary>
        ///     short string for this runtime value
        /// </summary>
        /// <returns></returns>
        public string InternalTypeFormat
            => "*";
    }
}