using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///
    /// </summary>
    public class TypeReference : ITypeReference {

        /// <summary>
        ///     reference to type
        /// </summary>
        /// <param name="typeId"></param>
        public TypeReference(int typeId)
            => TypeId = typeId;

        /// <summary>
        ///     reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.TypeName;

        /// <summary>
        ///     unknown type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.Type;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     get type format
        /// </summary>
        public string InternalTypeFormat
            => $"ref to {TypeId}";

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

    }
}