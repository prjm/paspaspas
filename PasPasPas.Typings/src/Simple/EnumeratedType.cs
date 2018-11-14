using System.Collections.Generic;
using System.Linq;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Operators;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumerated type
    /// </summary>
    public class EnumeratedType : OrdinalTypeBase, IOrdinalType {

        private readonly object lockObject = new object();

        /// <summary>
        ///     list of possible values
        /// </summary>
        private readonly IList<EnumValue> values
            = new List<EnumValue>();

        /// <summary>
        ///     create an enumerated type
        /// </summary>
        /// <param name="withId">type id</param>
        public EnumeratedType(int withId) : base(withId) {
        }

        /// <summary>
        ///     enumerated type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.EnumerationType;

        /// <summary>
        ///     get enumeration values
        /// </summary>
        public IList<EnumValue> Values
            => values;

        /// <summary>
        ///     get the required type id for this enumerated type
        /// </summary>
        public int CommonTypeId {
            get {
                var result = KnownTypeIds.ShortInt;
                var unsigned = true;

                foreach (var value in Values) {

                    if (value.Value is IEnumeratedValue enumValue)
                        return enumValue.Value.TypeId;

                    unsigned = unsigned && TypeRegistry.Runtime.AreValuesUnsigned(value.Value, value.Value);
                    result = TypeRegistry.GetSmallestIntegralTypeOrNext(result, value.Value.TypeId, 8, unsigned);
                }

                return result;
            }
        }

        private ITypeReference highestElement;
        private ITypeReference lowestElement;

        /// <summary>
        ///     highest element
        /// </summary>
        public ITypeReference HighestElement {
            get {
                lock (lockObject) {
                    if (highestElement != default || values.Count < 1)
                        return default;

                    highestElement = values[0].Value;

                    for (var i = 1; i < values.Count; i++) {
                        var result = TypeRegistry.Runtime.Integers.GreaterThen(values[i].Value, highestElement);
                        if (!(result is IBooleanValue boleanResult))
                            return default;

                        if (boleanResult.AsBoolean)
                            highestElement = values[i].Value;
                    }

                    return highestElement;
                }
            }
        }

        /// <summary>
        ///     lowest element
        /// </summary>
        public ITypeReference LowestElement {
            get {
                lock (lockObject) {
                    if (lowestElement != default || values.Count < 1)
                        return default;

                    lowestElement = values[0].Value;

                    for (var i = 1; i < values.Count; i++) {
                        var result = TypeRegistry.Runtime.Integers.LessThen(values[i].Value, lowestElement);
                        if (!(result is IBooleanValue boleanResult))
                            return default;

                        if (boleanResult.AsBoolean)
                            lowestElement = values[i].Value;
                    }

                    return lowestElement;
                }
            }
        }

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize {
            get {
                var type = TypeRegistry.GetTypeByIdOrUndefinedType(TypeRegistry.GetSmallestIntegralTypeOrNext(LowestElement.TypeId, HighestElement.TypeId)) as IIntegralType;

                if (type != default)
                    return type.BitSize;
                else
                    return 0;
            }
        }

        /// <summary>
        ///     define a new enumeration value
        /// </summary>
        /// <param name="runtimeValues">runtime values</param>
        /// <param name="symbolName">symbol name</param>
        /// <param name="withValue">if <c>true</c> a value definition is used</param>
        /// <param name="enumValue">optional value definition</param>
        public IRefSymbol DefineEnumValue(IRuntimeValueFactory runtimeValues, string symbolName, bool withValue, ITypeReference enumValue) {
            ITypeReference newValue;

            if (withValue)
                newValue = enumValue;
            else if (values.Count > 0)
                newValue = runtimeValues.Integers.Increment(values.Last().Value);
            else
                newValue = runtimeValues.Integers.Zero;

            if (!newValue.IsConstant)
                return null;

            var enumValueDefinition = new EnumValue(symbolName, newValue);
            values.Add(enumValueDefinition);
            return enumValueDefinition;
        }

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

        /// <summary>
        ///     readable type name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Enum {TypeRegistry.GetTypeByIdOrUndefinedType(CommonTypeId)}";
    }
}
