using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     ANSI char type (1 byte)
    /// </summary>
    public class AnsiCharType : OrdinalTypeBase, ICharType {

        /// <summary>
        ///     create a new char type
        /// </summary>
        /// <param name="withId">type id</param>
        public AnsiCharType(int withId) : base(withId) {
        }

        /// <summary>
        ///     type kind: ANSI char type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.AnsiCharType;

        /// <summary>
        ///     highest element
        /// </summary>
        public ulong HighestElement
            => byte.MaxValue;

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize
            => 8;

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

        /// <summary>
        ///     readable type name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "AnsiChar";

    }
}
