using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     meta class type
    /// </summary>
    public class MetaClassType : IMetaClassType {

        /// <summary>
        ///     create a new meta class type
        /// </summary>
        /// <param name="types"></param>
        /// <param name="baseType"></param>
        /// <param name="typeId">type id</param>
        public MetaClassType(RegisteredTypes types, int typeId, int baseType) {
            TypeRegistry = types;
            BaseTypeId = baseType;
            TypeId = typeId;
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.MetaClassType;

        /// <summary>
        ///     registered types
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     short type name
        /// </summary>
        public string ShortName
            => "";

        /// <summary>
        ///     long type name
        /// </summary>
        public string LongName
            => "";

        /// <summary>
        ///     test assignments
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public bool CanBeAssignedFrom(ITypeDefinition otherType)
            => false;
    }
}
