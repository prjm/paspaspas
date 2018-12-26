using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type definition
    /// </summary>
    public class RealType : TypeBase {

        /// <summary>
        ///     real type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withBitSize"></param>
        public RealType(int withId, uint withBitSize) : base(withId)
            => BitSize = withBitSize;

        /// <summary>
        ///     common type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.RealType;

        /// <summary>
        ///     bitsize
        /// </summary>
        public uint BitSize { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => (BitSize - 1) / 8u + 1u;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.RealType)
                return true;

            if (otherType.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (otherType.TypeKind == CommonTypeKind.IntegerType)
                return true;

            return base.CanBeAssignedFrom(otherType);
        }


    }
}
