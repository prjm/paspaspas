using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     structured type operations
    /// </summary>
    public class StructuredTypeOperations : IStructuredTypeOperations {

        /// <summary>
        ///     create a new array value
        /// </summary>
        /// <param name="registeredType"></param>
        /// <param name="baseTypeId"></param>
        /// <returns></returns>
        public IArrayValue CreateArrayValue(int registeredType, int baseTypeId)
            => new ArrayValue(registeredType, baseTypeId);
    }
}
