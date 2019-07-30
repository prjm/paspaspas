namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     compiler options
    /// </summary>
    public interface ICompilerOptions {

        /// <summary>
        ///     code generation options
        /// </summary>
        ICodeGenerationOptions CodeGeneration { get; }

        /// <summary>
        ///     link options
        /// </summary>
        ILinkOptions LinkOptions { get; }

        /// <summary>
        ///     debug options
        /// </summary>
        IDebugOptions DebugOptions { get; }

        /// <summary>
        ///     syntax options
        /// </summary>
        ISyntaxOptions Syntax { get; }

        /// <summary>
        ///     additional options
        /// </summary>
        IAdditionalOptions AdditionalOptions { get; }

        /// <summary>
        ///     hints and warnings
        /// </summary>
        IHintsAndWarnigs HintsAndWarnings { get; }

        /// <summary>
        ///     runtime options
        /// </summary>
        IRuntimeOptions RuntimeChecks { get; }

        /// <summary>
        ///     clear the options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options for a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}