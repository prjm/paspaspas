namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     helper for constant operations
    /// </summary>
    public interface IConstantOperations {

        /// <summary>
        ///     negate a value
        /// </summary>
        /// <param name="value">value to negage</param>
        /// <returns></returns>
        object Negate(object value);

        /// <summary>
        ///     convert an integer to the appropriate runtime constant
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        object ToConstantInt(long result);
    }
}