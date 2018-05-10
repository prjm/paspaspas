using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     int64 types
    /// </summary>
    public class Integral64BitType : OrdinalTypeBase, IIntegralType {

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
        ///     get the highest element of this ordinal type
        /// </summary>
        public ulong HighestElement {
            get {
                if (IsSigned)
                    return 9223372036854775807;
                return 18446744073709551615;
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

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{(IsSigned ? string.Empty : "U")}Int64";
    }
}
