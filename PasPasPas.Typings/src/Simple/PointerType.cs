using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     pointer type definition
    /// </summary>
    public class PointerType : TypeBase {

        /// <summary>
        ///     create a new pointer type definition
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type id</param>
        /// <param name="longTypeName">pointer type name</param>
        public PointerType(int withId, int baseType, string longTypeName = "") : base(withId) {
            BaseTypeId = baseType;
            LongName = longTypeName;
        }

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

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; }

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName { get; }

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName {
            get {
                if (BaseTypeId == KnownTypeIds.UntypedPointer)
                    return KnownNames.PV;
                var baseType = TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId);
                return string.Concat(KnownNames.P, baseType.ShortName);
            }
        }
    }
}
