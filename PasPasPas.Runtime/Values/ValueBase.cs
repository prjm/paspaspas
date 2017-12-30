using PasPasPas.Global.Runtime;

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

        /// <summary>
        ///     compare to equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        ///     get the hash code
        /// </summary>
        /// <returns></returns>
        public abstract override int GetHashCode();
    }
}
