using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type defnition
    /// </summary>
    public class RealType : TypeBase {

        /// <summary>
        ///     real type
        /// </summary>
        /// <param name="withId">type id</param>
        public RealType(int withId) : base(withId) {
        }

        /// <summary>
        ///     common type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.FloatType;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.FloatType)
                return true;

            return base.CanBeAssignedFrom(otherType);
        }


    }
}
