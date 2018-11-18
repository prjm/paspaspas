using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///
    /// </summary>
    public class TypeReference : ITypeNameReference {

        /// <summary>
        ///     reference to type
        /// </summary>
        /// <param name="typeId"></param>
        public TypeReference(int typeId)
            => BaseTypeId = typeId;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     base type
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId);

        /// <summary>
        ///     type id
        /// </summary>
        public int BaseTypeId { get; }

        /// <summary>
        ///     reference to types
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.Type;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => KnownTypeIds.Type;

        /// <summary>
        ///     format type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "ref " + BaseType.ToString();

    }
}