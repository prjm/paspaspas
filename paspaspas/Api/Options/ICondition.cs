namespace PasPasPas.Api.Options {

    /// <summary>
    ///     interface for elements controlling conditional compilation
    /// </summary>
    public interface ICondition {


        /// <summary>
        ///      test if the condition matches
        /// </summary>
        bool Matches { get; set; }

    }
}