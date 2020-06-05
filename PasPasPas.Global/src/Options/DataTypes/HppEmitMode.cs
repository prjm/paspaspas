#nullable disable
namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     emit mode for c++ headers
    /// </summary>
    public enum HppEmitMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     emit at header start
        /// </summary>
        Standard = 1,

        /// <summary>
        ///     emit at header end
        /// </summary>
        AtEnd = 2,

        /// <summary>
        ///     generate link unit directive
        /// </summary>
        LinkUnit = 3,

        /// <summary>
        ///     open namespace
        /// </summary>
        OpenNamespace = 4,

        /// <summary>
        ///     close namespace
        /// </summary>
        CloseNamespace = 5,

        /// <summary>
        ///     supress namespace
        /// </summary>
        NoUsingNamespace = 6,
    }
}