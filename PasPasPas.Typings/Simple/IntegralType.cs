using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    public class IntegralType : OrdinalTypeBase, IIntegralType {

        private readonly bool signed;
        private readonly uint bitSize;

        /// <summary>
        ///     create a new type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="isSigned"><c>true</c> if the type is signed</param>
        /// <param name="withBitSize">bit size of the type</param>
        public IntegralType(int withId, bool isSigned, uint withBitSize) : base(withId) {
            signed = isSigned;
            bitSize = withBitSize;
        }

        /// <summary>
        ///     integer type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.IntegerType;

        /// <summary>
        ///     check if this type is signed
        /// </summary>
        public bool IsSigned
            => signed;

        /// <summary>
        ///     get the size in bits
        /// </summary>
        public uint BitSize
            => bitSize;

        /// <summary>
        ///     get the highest element
        /// </summary>
        public ulong HighestElement {
            get {
                if (signed) {
                    if (BitSize == 8)
                        return 127;
                    else if (BitSize == 16)
                        return 32767;

                    return 2147483647;
                }
                else {
                    if (BitSize == 8)
                        return 255;
                    else if (BitSize == 16)
                        return 65535;

                    return 4294967295;
                }
            }
        }

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.IntegerType)
                return true;

            if (otherType.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.IntegerType || subrangeBase.TypeKind == CommonTypeKind.Int64Type;
            }

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     short info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{(signed ? string.Empty : "U")}Int{BitSize}";

    }
}
