namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     interface for elements controlling conditional compilation
    /// </summary>
    public interface ICondition {

        /// <summary>
        ///      test if the condition matches
        /// </summary>
        bool Matches { get; }

    }
}