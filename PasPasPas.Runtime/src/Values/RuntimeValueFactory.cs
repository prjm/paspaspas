using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     runtime values: value creation and operations
    /// </summary>
    public partial class RuntimeValueFactory : IRuntimeValueFactory {

        private IOldTypeReference MakeSubrangeValue(int typeId, IOldTypeReference value)
            => Types.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOldTypeReference MakeEnumValue(int typeId, IOldTypeReference value)
            => new EnumeratedValue(typeId, value);

        /// <summary>
        ///     make a pointer value
        /// </summary>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        public IOldTypeReference MakePointerValue(IOldTypeReference baseValue)
            => new PointerValue(KnownTypeIds.GenericPointer, baseValue);
    }
}