using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     character based type
    /// </summary>
    public class AnsiCharType : OrdinalTypeBase {

        /// <summary>cccy
        ///     create a new char type
        /// </summary>
        /// <param name="withId">type id</param>
        public AnsiCharType(int withId) : base(withId) {
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.AnsiCharType;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.AnsiCharType;
            }

            return base.CanBeAssignedFrom(otherType);
        }
    }
}
