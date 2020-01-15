using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    public class IntegralType : OrdinalTypeBase, IIntegralType {
        private readonly object lockObject = new object();
        private IOldTypeReference highestElement;
        private IOldTypeReference lowestElement;

        /// <summary>
        ///     create a new type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="isSigned"><c>true</c> if the type is signed</param>
        /// <param name="withBitSize">bit size of the type</param>
        public IntegralType(int withId, bool isSigned, uint withBitSize) : base(withId) {
            IsSigned = isSigned;
            BitSize = withBitSize;
        }

        /// <summary>
        ///     integer type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.IntegerType;

        /// <summary>
        ///     check if this type is signed
        /// </summary>
        public bool IsSigned { get; }

        /// <summary>
        ///     get the size in bits
        /// </summary>
        public uint BitSize { get; }

        /// <summary>
        ///     get the highest element
        /// </summary>
        private IOldTypeReference GenerateHighestElement() {
            var ints = TypeRegistry.Runtime.Integers;

            if (IsSigned) {
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
        private IOldTypeReference GenerateLowestElement() {
            var ints = TypeRegistry.Runtime.Integers;

            if (!IsSigned)
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
        public IOldTypeReference HighestElement {
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
        public IOldTypeReference LowestElement {
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
        ///     get the long type name
        /// </summary>
        public override string LongName {
            get {
                switch (BitSize) {
                    case 8:
                        return IsSigned ? KnownNames.ShortInt : KnownNames.Byte;
                    case 16:
                        return IsSigned ? KnownNames.SmallInt : KnownNames.Word;
                    case 32:
                        return IsSigned ? KnownNames.Integer : KnownNames.Cardinal;
                }

                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     get the short type name
        /// </summary>
        public override string ShortName {
            get {
                switch (BitSize) {
                    case 8:
                        return IsSigned ? KnownNames.ZC : KnownNames.UC;
                    case 16:
                        return IsSigned ? KnownNames.S : KnownNames.US;
                    case 32:
                        return IsSigned ? KnownNames.I : KnownNames.UI;
                }

                throw new InvalidOperationException();
            }
        }
    }
}
