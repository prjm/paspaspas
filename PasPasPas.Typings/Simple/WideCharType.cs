using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     wide char type
    /// </summary>
    public class WideCharType : OrdinalTypeBase, ICharType {

        /// <summary>
        ///     wide char type
        /// </summary>
        /// <param name="withId"></param>
        public WideCharType(int withId) : base(withId) {
        }

        /// <summary>
        ///     wide char type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideCharType;

        /// <summary>
        ///     highest element
        /// </summary>
        public ulong HighestElement
            => ushort.MaxValue;

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize
            => 16;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.WideCharType;
            }

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     readable type name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "WideChar";

    }
}
