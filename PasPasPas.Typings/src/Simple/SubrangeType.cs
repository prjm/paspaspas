using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     subrange type
    /// </summary>
    public class SubrangeType : TypeBase, ISubrangeType {

        /// <summary>
        ///     create a new subrange type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type</param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public SubrangeType(int withId, int baseType, ITypeReference low, ITypeReference high) : base(withId) {
            BaseTypeId = baseType;
            LowestElement = low;
            HighestElement = high;
        }

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.SubrangeType;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType otherSubrange) {


            }

            if (BaseType.CanBeAssignedFrom(otherType)) {
                return true;
            }

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; }

        /// <summary>
        ///     base type
        /// </summary>
        public IOrdinalType BaseType {
            get {
                var result = TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId) as IOrdinalType;
                return result;
            }
        }

        /// <summary>
        ///     highest element
        /// </summary>
        public ITypeReference HighestElement { get; }

        /// <summary>
        ///     lowest element
        /// </summary>
        public ITypeReference LowestElement { get; }

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize
            => BaseType.BitSize;

        /// <summary>
        ///     format this subrange type
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => StringUtils.Invariant($"Subrange {BaseType}");

    }
}