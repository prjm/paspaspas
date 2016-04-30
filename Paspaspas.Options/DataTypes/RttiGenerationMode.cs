namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     mode for rtti generation
    /// </summary>
    public enum RttiGenerationMode {

        /// <summary>
        ///     undefined rtti generation mode (use parent)
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     inherit rtti definitions from base class
        /// </summary>
        Inherit = 1,

        /// <summary>
        ///     explicit rtti definition
        /// </summary>
        Explicit = 2

    }
}