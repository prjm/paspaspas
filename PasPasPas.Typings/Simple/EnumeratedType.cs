using System.Collections.Generic;
using System.Linq;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

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
        ///     get enum values
        /// </summary>
        public IList<EnumValue> Values
            => values;

        /// <summary>
        ///     define an enum value
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <param name="withValue">if <c>true</c> a value definition is used</param>
        /// <param name="enumValue">optional value definition</param>
        public void DefineEnumValue(string symbolName, bool withValue, int enumValue) {
            int newValue;

            if (withValue)
                newValue = enumValue;
            else if (values.Count > 0)
                newValue = 1 + values.Last().Value;
            else
                newValue = 0;

            values.Add(new EnumValue(symbolName, enumValue));
        }

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeInfo.TypeId == TypeInfo.TypeId;
            }

            return base.CanBeAssignedFrom(otherType);
        }

    }
}
