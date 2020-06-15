using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     subrange type
    /// </summary>
    internal class SubrangeType : TypeDefinitionBase, ISubrangeType {

        /// <summary>
        ///     create a new subrange type
        /// </summary>
        /// <param name="definingUnit">defining unit</param>
        /// <param name="subrangeOf">subrange of another type</param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="name">type name</param>
        public SubrangeType(IUnitType definingUnit, string name, IOrdinalType subrangeOf, IValue low, IValue high) : base(definingUnit) {
            SubrangeOfType = subrangeOf;
            LowestElement = low;
            HighestElement = high;
            Name = name;
        }

        /// <summary>
        ///     mangled name
        /// </summary>
        public override string MangledName
            => SubrangeOfType.MangledName;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Subrange;

        /*
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
        */

        /// <summary>
        ///     subrange of another type
        /// </summary>
        public IOrdinalType SubrangeOfType { get; }

        /// <summary>
        ///     highest element
        /// </summary>
        public IValue HighestElement { get; }

        /// <summary>
        ///     lowest element
        /// </summary>
        public IValue LowestElement { get; }

        /// <summary>
        ///     test if this type is signed
        /// </summary>
        public bool IsSigned
            => SubrangeOfType.IsSigned;

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => SubrangeOfType.TypeSizeInBytes;

        /// <summary>
        ///     test if this type definition is valid
        /// </summary>
        public bool IsValid {
            get {
                if (LowestElement == default || HighestElement == default)
                    return false;

                if (!(LowestElement is IOrdinalType _))
                    return false;

                if (!(HighestElement is IOrdinalValue _))
                    return false;

                return true;
            }
        }

        /// <summary>
        ///     compute cardinality
        /// </summary>
        public BigInteger Cardinality {
            get {
                if (!(LowestElement is IOrdinalValue lowerValue))
                    return BigInteger.Zero;

                if (!(HighestElement is IOrdinalValue highValue))
                    return BigInteger.Zero;

                var low = lowerValue.GetOrdinalValue(TypeRegistry);
                var high = highValue.GetOrdinalValue(TypeRegistry);

                if (!(low is IIntegerValue l))
                    return BigInteger.Zero;

                if (!(high is IIntegerValue h))
                    return BigInteger.Zero;

                return BigInteger.Add(BigInteger.One, BigInteger.Subtract(h.AsBigInteger, l.AsBigInteger));
            }
        }

        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is ISubrangeType s &&
                s.SubrangeOfType.Equals(SubrangeOfType) &&
                s.LowestElement.Equals(LowestElement) &&
                s.HighestElement.Equals(HighestElement);
    }
}