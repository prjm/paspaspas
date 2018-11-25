using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumeration value declaration
    /// </summary>
    public class EnumValue : IRefSymbol {

        /// <summary>
        ///     create a new enumeration value
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <param name="value">symbol value</param>
        public EnumValue(string name, ITypeReference value) {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     name of the enumeration item
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     value of the enumerated item
        /// </summary>
        public ITypeReference Value { get; private set; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => Value.TypeId;

        /// <summary>
        ///     convert the value of this enumeration to an enumerated value
        /// </summary>
        /// <param name="runtime">runtime values</param>
        /// <param name="typeId">given type id</param>
        /// <param name="types">type registry</param>
        /// <param name="enumTypeId"></param>
        public void MakeEnumValue(IRuntimeValueFactory runtime, ITypeRegistry types, int typeId, int enumTypeId)
            => Value = runtime.Types.MakeEnumValue(enumTypeId, runtime.Cast(types, Value, typeId));
    }
}