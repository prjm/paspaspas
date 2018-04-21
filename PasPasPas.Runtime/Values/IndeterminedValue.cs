using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     indetermined value
    /// </summary>
    public class IndeterminedValue : IValue {

        private readonly int typeId;

        /// <summary>
        ///     create a new indermined value
        /// </summary>
        /// <param name="typeId"></param>
        public IndeterminedValue(int typeId) => this.typeId = typeId;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => typeId;
    }
}