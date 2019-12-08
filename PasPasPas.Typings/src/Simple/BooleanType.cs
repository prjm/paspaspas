using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     boolean type
    /// </summary>
    public class BooleanType : OrdinalTypeBase, IBooleanType {

        /// <summary>
        ///     create a new boolean type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="bitSize">size in bits</param>
        public BooleanType(int withId, uint bitSize) : base(withId)
            => BitSize = bitSize;

        /// <summary>
        ///     enumerated type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.BooleanType;

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize { get; }

        /// <summary>
        ///     true value
        /// </summary>
        public ITypeReference HighestElement
            => TypeRegistry.Runtime.Booleans.TrueValue;

        /// <summary>
        ///     false value
        /// </summary>
        public ITypeReference LowestElement
            => TypeRegistry.Runtime.Booleans.FalseValue;

        /// <summary>
        ///     unsigned data type
        /// </summary>
        public bool IsSigned
            => false;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => (BitSize - 1) / 8u + 1u;

        /// <summary>
        ///     type kind
        /// </summary>
        public BooleanTypeKind Kind
            => BooleanTypeKind.Boolean;

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

        /// <summary>
        ///     convert this type to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            switch (BitSize) {
                case 1:
                    return "Boolean";
                case 8:
                    return "ByteBool";
                case 16:
                    return "WordBool";
                default:
                    return $"Bool{BitSize}";
            }
        }
    }
}
