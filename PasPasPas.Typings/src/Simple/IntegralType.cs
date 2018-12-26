using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    public class IntegralType : OrdinalTypeBase, IIntegralType {

        private readonly bool signed;
        private readonly uint bitSize;
        private readonly object lockObject = new object();
        private ITypeReference highestElement;
        private ITypeReference lowestElement;

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
        private ITypeReference GenerateHighestElement() {
            var ints = TypeRegistry.Runtime.Integers;

            if (signed) {
                if (BitSize == 8)
                    return ints.ToScaledIntegerValue(127);
                else if (BitSize == 16)
                    return ints.ToScaledIntegerValue(32767);
                else if (BitSize == 32)
                    return ints.ToScaledIntegerValue(2147483647);
            }

            if (BitSize == 8)
                return ints.ToScaledIntegerValue(255);
            else if (BitSize == 16)
                return ints.ToScaledIntegerValue(65535);
            else if (BitSize == 32)
                return ints.ToScaledIntegerValue(4294967295);

            return ints.Invalid;
        }

        /// <summary>
        ///     get the highest element
        /// </summary>
        private ITypeReference GenerateLowestElement() {
            var ints = TypeRegistry.Runtime.Integers;

            if (!signed)
                return ints.Zero;

            if (BitSize == 8)
                return ints.ToScaledIntegerValue(-128);

            else if (BitSize == 16)
                return ints.ToScaledIntegerValue(-32768);

            else if (BitSize == 32)
                return ints.ToScaledIntegerValue(-2147483648);

            return ints.Invalid;
        }

        /// <summary>
        ///     highest element
        /// </summary>
        public ITypeReference HighestElement {
            get {
                lock (lockObject) {
                    if (highestElement == default)
                        highestElement = GenerateHighestElement();
                    return highestElement;
                }
            }
        }

        /// <summary>
        ///     lowest element
        /// </summary>
        public ITypeReference LowestElement {
            get {
                lock (lockObject) {
                    if (lowestElement == default)
                        lowestElement = GenerateLowestElement();
                    return lowestElement;
                }
            }
        }

        /// <summary>
        ///     size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => BitSize / 8;

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
