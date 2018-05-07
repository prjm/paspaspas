using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumeration value declaration
    /// </summary>
    public class EnumValue : IRefSymbol {

        private readonly string symbolName;
        private ITypeReference enumValue;

        /// <summary>
        ///     create a new enumeration value
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <param name="value">symbol value</param>
        public EnumValue(string name, IValue value) {
            symbolName = name;
            enumValue = value;
        }

        /// <summary>
        ///     name of the enumeration item
        /// </summary>
        public string Name
            => symbolName;

        /// <summary>
        ///     value of the enumerated item
        /// </summary>
        public ITypeReference Value
            => enumValue;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => enumValue.TypeId;

        /// <summary>
        ///     convert the value of this enumeration to an enumerated value
        /// </summary>
        /// <param name="runtime">runtime values</param>
        /// <param name="typeId">given type id</param>
        public void MakeEnumValue(IRuntimeValueFactory runtime, int typeId, int enumTypeId) {
            enumValue = runtime.Types.MakeEnumValue(enumTypeId, runtime.Integers.Cast(enumValue, typeId));
        }
    }
}