using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     int64 types
    /// </summary>
    public class Integral64BitType : OrdinalTypeBase, IIntegralType {

        private readonly object lockObject = new object();
        private ITypeReference highestElement;
        private ITypeReference lowestElement;

        /// <summary>
        ///     create a new int64 type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="isSigned"><c>true</c> if this type should be signed</param>
        public Integral64BitType(int withId, bool isSigned) : base(withId)
            => IsSigned = isSigned;

        /// <summary>
        ///     common type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Int64Type;

        /// <summary>
        ///     check if this type is signed
        /// </summary>
        public bool IsSigned { get; }

        /// <summary>
        ///     bis size of this type (64 bit)
        /// </summary>
        public uint BitSize
            => 64;

        /// <summary>
        ///     highest element: <c>0xff</c>
        /// </summary>
        public ITypeReference HighestElement {
            get {
                lock (lockObject) {
                    if (highestElement == default) {
                        if (IsSigned)
                            highestElement = TypeRegistry.Runtime.Integers.ToIntegerValue(9223372036854775807);
                        else
                            highestElement = TypeRegistry.Runtime.Integers.ToIntegerValue(18446744073709551615);
                    }
                    return highestElement;
                }
            }
        }

        /// <summary>
        ///     lowest element: <c>0</c>
        /// </summary>
        public ITypeReference LowestElement {
            get {
                lock (lockObject) {
                    if (lowestElement == default) {
                        if (IsSigned)
                            lowestElement = TypeRegistry.Runtime.Integers.ToIntegerValue(-9223372036854775808);
                        else
                            lowestElement = TypeRegistry.Runtime.Integers.Zero;
                    }
                    return lowestElement;
                }
            }
        }

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 8;

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

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{(IsSigned ? string.Empty : "U")}Int64";

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName
            => IsSigned ? KnownNames.Int64 : KnownNames.UInt64;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName
            => IsSigned ? KnownNames.J : KnownNames.UJ;


    }
}
