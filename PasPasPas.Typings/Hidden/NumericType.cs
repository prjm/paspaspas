using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     hidden numeric type
    /// </summary>
    public class NumericType : HiddenIntrinsicType {

        /// <summary>
        ///     create a generic numeric tyep
        /// </summary>
        /// <param name="withId"></param>
        public NumericType(int withId) : base(withId) {
        }

        /// <summary>
        ///     test if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType">other type</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind.IsNumerical())
                return true;

            return base.CanBeAssignedFrom(otherType);
        }
    }
}
