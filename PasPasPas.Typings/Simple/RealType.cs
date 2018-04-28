using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type definition
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
            => CommonTypeKind.RealType;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.RealType)
                return true;

            if (otherType.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (otherType.TypeKind == CommonTypeKind.IntegerType)
                return true;

            return base.CanBeAssignedFrom(otherType);
        }


    }
}
