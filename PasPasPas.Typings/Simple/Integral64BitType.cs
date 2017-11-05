using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     int64 types
    /// </summary>
    public class Integral64BitType : TypeBase, IIntegralType {

        private readonly bool signed;

        /// <summary>
        ///     create a new int64 type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="isSigned"><c>true</c> if this type should be signed</param>
        /// <param name="withName">type name (optional)</param>
        public Integral64BitType(int withId, bool isSigned, ScopedName withName = null) : base(withId, withName)
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
    }
}
