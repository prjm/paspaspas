using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     runtime values: value creation and operations
    /// </summary>
    public partial class RuntimeValueFactory : IRuntimeValueFactory {

        private IValue MakeSubrangeValue(ITypeDefinition typeId, IValue value)
            => Types.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <param name="name">value name</param>
        /// <returns></returns>
        public IEnumeratedValue MakeEnumValue(ITypeDefinition typeId, IIntegerValue value, string name)
            => new EnumeratedValue(typeId, value, name);

        /// <summary>
        ///     make a pointer value
        /// </summary>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        public IValue MakePointerValue(IValue baseValue)
            => new PointerValue(typeRegistryProvider.GetGenericPointerType(), baseValue);

        /// <summary>
        ///     get the value of value object as string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetValueString(IValue value)
            => value.GetValueString();
    }
}