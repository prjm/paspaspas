using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enumeration value declaration
    /// </summary>
    public class EnumValue : IEnumeratedValue {

        /// <summary>
        ///     create a new enumeration value
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <param name="value">symbol value</param>
        public EnumValue(string name, IValue value) {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => Value.TypeDefinition;

        /// <summary>
        ///     name of the enumeration item
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     value of the enumerated item
        /// </summary>
        public IValue Value { get; private set; }

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