using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Operators;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumerated type
    /// </summary>
    public class EnumeratedType : OrdinalTypeBase {

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
                    unsigned = unsigned && TypeRegistry.Runtime.AreValuesUnsigned(value.Value, value.Value);
                    result = TypeRegistry.GetSmallestIntegralTypeOrNext(result, value.Value.TypeId, 8, unsigned);
                }

                return result;
            }
        }

        /// <summary>
        ///     define a new enumeration value
        /// </summary>
        /// <param name="runtimeValues">runtime values</param>
        /// <param name="symbolName">symbol name</param>
        /// <param name="withValue">if <c>true</c> a value definition is used</param>
        /// <param name="enumValue">optional value definition</param>
        public IRefSymbol DefineEnumValue(IRuntimeValueFactory runtimeValues, string symbolName, bool withValue, IValue enumValue) {
            ITypeReference newValue;

            if (withValue)
                newValue = enumValue;
            else if (values.Count > 0)
                newValue = runtimeValues.Integers.Increment(values.Last().Value);
            else
                newValue = runtimeValues.Integers.Zero;

            if ((!newValue.IsConstant) || (!(newValue is IValue constValue)))
                return null;

            var enumValueDefinition = new EnumValue(symbolName, constValue);
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
        public override string ToString()
            => "Enum";

    }
}
