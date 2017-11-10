using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumerated type
    /// </summary>
    public class EnumeratedType : TypeBase {

        /// <summary>
        ///     list of possible values
        /// </summary>
        private readonly IList<EnumValue> values
            = new List<EnumValue>();

        /// <summary>
        ///     create an enumerated type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withName">type name</param>
        public EnumeratedType(int withId, ScopedName withName = null) : base(withId, withName) {
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
    }
}
