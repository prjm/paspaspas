using PasPasPas.Infrastructure.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     base class for values
    /// </summary>
    public abstract class ValueBase : IValue {

        /// <summary>
        ///     get the data of this value
        /// </summary>
        public abstract byte[] Data { get; }

        /// <summary>
        ///     show a visual description of this value
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        ///     get the well known type id
        /// </summary>
        public abstract int TypeId { get; }
    }
}
