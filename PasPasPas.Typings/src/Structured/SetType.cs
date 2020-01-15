using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     set type declaration
    /// </summary>
    public class SetType : StructuredTypeBase, ISetType {

        /// <summary>
        ///     define a new set type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type</param>
        public SetType(int withId, int baseType) : base(withId)
            => BaseTypeId = baseType;

        /// <summary>
        ///     set type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.SetType;

        /// <summary>
        ///     base type
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId);

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                IOldTypeReference div8(IOldTypeReference v)
                    => TypeRegistry.Runtime.Integers.Divide(v, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(8));

                var enumType = BaseType as EnumeratedType;
                var lowest = enumType.LowestElement as IOrdinalValue;
                var highest = enumType.HighestElement as IOrdinalValue;
                var l = lowest.GetOrdinalValue(TypeRegistry);
                var h = highest.GetOrdinalValue(TypeRegistry);

                var size0 = TypeRegistry.Runtime.Integers.Subtract(div8(h), div8(l));
                var size1 = TypeRegistry.Runtime.Integers.Add(size0, TypeRegistry.Runtime.Integers.One) as IIntegerValue;
                return System.Math.Max(0, (uint)size1.SignedValue);
            }
        }

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; }

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

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"set of {BaseType}";

    }
}
