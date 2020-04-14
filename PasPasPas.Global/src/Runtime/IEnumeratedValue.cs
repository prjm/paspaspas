using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value of an enumeration
    /// </summary>
    public interface IEnumeratedValue : IValue {

        /// <summary>
        ///     constant value
        /// </summary>
        IIntegerValue Value { get; }

        /// <summary>
        ///     make an enumerated value
        /// </summary>
        /// <param name="types"></param>
        /// <param name="typeId"></param>
        /// <param name="enumTypeId"></param>
        void MakeEnumValue(ITypeRegistry types, ITypeDefinition typeId, ITypeDefinition enumTypeId);
    }
}
