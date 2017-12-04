using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     boolean type
    /// </summary>
    public class BooleanType : OrdinalTypeBase {
        private readonly int size;

        /// <summary>
        ///     create a new boolean type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="bitSize">size in bits</param>
        public BooleanType(int withId, int bitSize) : base(withId)
            => this.size = bitSize;

        /// <summary>
        ///     enumerated type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.BooleanType;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.BooleanType;
            }

            return base.CanBeAssignedFrom(otherType);
        }

    }
}
