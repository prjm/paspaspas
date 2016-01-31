namespace PasPasPas.Api {

    /// <summary>
    ///     interface for syntax elements
    /// </summary>
    public interface ISyntaxPart {

        /// <summary>
        ///     print a pascal representation of this program part
        /// </summary>        
        /// <param name="result">output</param>
        void ToFormatter(PascalFormatter result);
    }
}