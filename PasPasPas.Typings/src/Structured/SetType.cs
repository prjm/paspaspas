using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     set type declaration
    /// </summary>
    internal class SetType : StructuredTypeBase, ISetType {

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
        public string Name { get; }

        /// <summary>
        ///     mangled name
        /// </summary>
        public string MangledName
            => string.Concat(DefiningUnit.Name, KnownNames.AtSymbol, Name);


        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                IValue div8(IValue v)
                    => TypeRegistry.Runtime.Integers.Divide(v, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(8));

                var enumType = BaseTypeDefinition;
                var lowest = enumType.LowestElement as IOrdinalValue ?? throw new InvalidOperationException();
                var highest = enumType.HighestElement as IOrdinalValue ?? throw new InvalidOperationException();
                var l = lowest.GetOrdinalValue(TypeRegistry);
                var h = highest.GetOrdinalValue(TypeRegistry);

                var size0 = TypeRegistry.Runtime.Integers.Subtract(div8(h), div8(l));
                var size1 = TypeRegistry.Runtime.Integers.Add(size0, TypeRegistry.Runtime.Integers.One) as IIntegerValue;
                return System.Math.Max(0, ((uint?)size1?.SignedValue) ?? 0u);
            }
        }

        /// <summary>
        ///     base type id
        /// </summary>
        public IOrdinalType BaseTypeDefinition { get; }

        public override bool Equals(ITypeDefinition? other)
            => other is ISetType s &&
                s.BaseTypeDefinition.Equals(BaseTypeDefinition);

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
