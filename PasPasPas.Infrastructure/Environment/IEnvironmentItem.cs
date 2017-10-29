namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     maintenance interface for environment items
    /// </summary>
    public interface IEnvironmentItem {

        /// <summary>
        ///     get item count
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     item caption
        /// </summary>
        string Caption { get; }


    }
}
