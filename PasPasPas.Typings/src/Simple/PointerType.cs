using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     pointer type definition
    /// </summary>
    public class PointerType : TypeBase {

        private readonly int baseTypeId;

        /// <summary>
        ///     create a new pointer type definition
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type id</param>
        public PointerType(int withId, int baseType) : base(withId)
            => baseTypeId = baseType;

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.PointerType;

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt).TypeSizeInBytes;

    }
}
