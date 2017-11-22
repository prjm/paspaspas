using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    public class IntegralType : OrdinalTypeBase, IIntegralType {

        private readonly bool signed;
        private readonly int bitSize;

        /// <summary>
        ///     create a new type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="isSigned"><c>true</c> if the type is signed</param>
        /// <param name="withBitSize">bitsize of the type</param>
        public IntegralType(int withId, bool isSigned, int withBitSize) : base(withId) {
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
        public bool Signed
            => signed;

        /// <summary>
        ///     get the size in bits
        /// </summary>
        public int BitSize
            => bitSize;
    }
}
