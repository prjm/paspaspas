#nullable disable
namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     binary reference for serialization
    /// </summary>
    public class Reference {

        /// <summary>
        ///     <c>true</c> if an address has been set
        /// </summary>
        public bool HasAddress { get; internal set; }
            = false;

        /// <summary>
        ///     internal address
        /// </summary>
        public long Address { get; internal set; }
            = -1;
    }
}
