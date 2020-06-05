#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     set type declaration
    /// </summary>
    public class SetType : StructuredTypeBase, ISetType {

        /// <summary>
        ///     define a new set type
        /// </summary>
        /// <param name="definingType">type id</param>
        /// <param name="baseType">base type</param>
        /// <param name="name">type name</param>
        public SetType(IUnitType definingType, string name, IOrdinalType baseType) : base(definingType) {
            Name = name;
            BaseTypeDefinition = baseType;
        }

        /// <summary>
        ///     set type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Set;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     mangled name
        /// </summary>
        public override string MangledName
            => string.Concat(DefiningUnit.Name, KnownNames.AtSymbol, Name);


        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                IValue div8(IValue v)
                    => TypeRegistry.Runtime.Integers.Divide(v, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(8));

                var enumType = BaseTypeDefinition as IOrdinalType;
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
        public IOrdinalType BaseTypeDefinition { get; }

        /*
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
        */

    }
}
