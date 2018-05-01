using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     set type declaration
    /// </summary>
    public class SetType : StructuredTypeBase {

        private readonly int baseTypeId;

        /// <summary>
        ///     define a new set type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type</param>
        public SetType(int withId, int baseType) : base(withId)
            => baseTypeId = baseType;

        /// <summary>
        ///     set type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.SetType;

        /// <summary>
        ///     base type
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeId);

        /// <summary>
        ///     check if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SetType && otherType is SetType set) {
                return BaseType.CanBeAssignedFrom(set.BaseType);
            }

            return base.CanBeAssignedFrom(otherType);
        }
    }
}
