using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     int64 types
    /// </summary>
    public class Integral64BitType : OrdinalTypeBase, IIntegralType {

        private readonly bool signed;

        /// <summary>
        ///     create a new int64 type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="isSigned"><c>true</c> if this type should be signed</param>
        public Integral64BitType(int withId, bool isSigned) : base(withId)
            => signed = isSigned;

        /// <summary>
        ///     common type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Int64Type;

        /// <summary>
        ///     check if this type is signed
        /// </summary>
        public bool Signed
            => signed;

        /// <summary>
        ///     bis size of this type (64 bit)
        /// </summary>
        public int BitSize
            => 64;

        /// <summary>
        ///     get the highest element of this ordinal type
        /// </summary>
        public ulong HighestElement {
            get {
                if (signed)
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

    }
}
