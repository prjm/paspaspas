using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumerated type
    /// </summary>
    internal class EnumeratedType : TypeDefinitionBase, IEnumeratedType {

        /// <summary>
        ///     list of possible values
        /// </summary>
        private readonly IList<IEnumeratedValue> values
            = new List<IEnumeratedValue>();

        /// <summary>
        ///     create an enumerated type
        /// </summary>
        /// <param name="definingUnit">defining unit</param>
        /// <param name="name"></param>
        public EnumeratedType(IUnitType definingUnit, string name) : base(definingUnit)
            => Name = name;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     mangled type name
        /// </summary>
        public override string MangledName
            => string.Concat(DefiningUnit.Name, KnownNames.AtSymbol, Name);

        /// <summary>
        ///     enumerated type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Enumeration;

        /// <summary>
        ///     get enumeration values
        /// </summary>
        public IList<IEnumeratedValue> Values
            => values;

        /// <summary>
        ///     highest element
        /// </summary>
        public IValue HighestElement {
            get {
                if (values.Count < 1)
                    return TypeRegistry.Runtime.Integers.Invalid;

                var highestElement = values[0].Value;

                for (var i = 1; i < values.Count; i++) {
                    var result = TypeRegistry.Runtime.Integers.GreaterThen(values[i].Value, highestElement);
                    if (!(result is IBooleanValue boleanResult))
                        return TypeRegistry.Runtime.Integers.Invalid;

                    if (boleanResult.AsBoolean)
                        highestElement = values[i].Value;
                }

                return highestElement;
            }
        }

        /// <summary>
        ///     lowest element
        /// </summary>
        public IValue LowestElement {
            get {
                if (values.Count < 1)
                    return TypeRegistry.Runtime.Integers.Invalid;

                var lowestElement = values[0].Value;

                for (var i = 1; i < values.Count; i++) {
                    var result = TypeRegistry.Runtime.Integers.LessThen(values[i].Value, lowestElement);
                    if (!(result is IBooleanValue boleanResult))
                        return TypeRegistry.Runtime.Integers.Invalid;

                    if (boleanResult.AsBoolean)
                        lowestElement = values[i].Value;
                }

                return lowestElement;
            }
        }

        /// <summary>
        ///     test if this type is signed
        /// </summary>
        public bool IsSigned {
            get {
                var type = CommonTypeId as IOrdinalType;

                if (type != default)
                    return type.IsSigned;
                else
                    return default;
            }
        }

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition CommonTypeId {
            get {
                var lowestElement = LowestElement;
                var lowerBaseType = TypeRegistry.SystemUnit.ErrorType as ITypeDefinition;
                var highestElement = HighestElement;
                var higherBaseType = TypeRegistry.SystemUnit.ErrorType as ITypeDefinition;

                if (lowestElement is IEnumeratedValue lowestEnumValue)
                    lowerBaseType = lowestEnumValue.Value.TypeDefinition;
                else if (lowestElement != default)
                    lowerBaseType = lowestElement.TypeDefinition;

                if (highestElement is IEnumeratedValue highestEnumValue)
                    higherBaseType = highestEnumValue.Value.TypeDefinition;
                else if (highestElement != default)
                    higherBaseType = highestElement.TypeDefinition;

                return TypeRegistry.GetSmallestIntegralTypeOrNext(lowerBaseType, higherBaseType);
            }
        }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => CommonTypeId.TypeSizeInBytes;

        /// <summary>
        ///     define a new enumeration value
        /// </summary>
        /// <param name="runtimeValues">runtime values</param>
        /// <param name="symbolName">symbol name</param>
        /// <param name="withValue">if <c>true</c> a value definition is used</param>
        /// <param name="enumValue">optional value definition</param>
        public IValue DefineEnumValue(IRuntimeValueFactory runtimeValues, string symbolName, bool withValue, IValue enumValue) {
            IValue newValue;

            if (withValue)
                newValue = enumValue;
            else if (values.Count > 0)
                newValue = runtimeValues.Integers.Increment(values.Last().Value);
            else
                newValue = runtimeValues.Integers.Zero;

            if (newValue is IIntegerValue integerValue) {
                var enumValueDefinition = runtimeValues.MakeEnumValue(this, integerValue, symbolName);
                values.Add(enumValueDefinition);
                return enumValueDefinition;
            }

            return runtimeValues.Integers.Invalid;
        }

        public override bool Equals(ITypeDefinition? other) {
            var otherEnum = other as IEnumeratedType;
            if (otherEnum == default)
                return false;

            if (KnownNames.SameIdentifier(Name, other?.Name))
                return false;

            if (otherEnum.Values.Count != values.Count)
                return false;

            for (var i = 0; i < values.Count; i++) {
                if (!string.Equals(values[i].Name, otherEnum.Values[i].Name, StringComparison.OrdinalIgnoreCase))
                    return false;

                if (!values[i].Equals(otherEnum.Values[i]))
                    return false;
            }

            return true;
        }

        /*
        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeId == TypeInfo.TypeId;
            }

            return base.CanBeAssignedFrom(otherType);
        }
        */

    }
}
