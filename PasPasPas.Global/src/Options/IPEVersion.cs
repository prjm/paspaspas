namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     pe version
    /// </summary>
    public interface IPEVersion {

        /// <summary>
        ///     major version
        /// </summary>
        IOption<int> MajorVersion { get; }

        /// <summary>
        ///     minor version
        /// </summary>
        IOption<int> MinorVersion { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();
    }
}
