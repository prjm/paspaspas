using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     debug options
    /// </summary>
    public interface IDebugOptions {

        /// <summary>
        ///     assertion types
        /// </summary>
        IOption<AssertionMode> Assertions { get; }

        /// <summary>
        ///     debug information
        /// </summary>
        IOption<DebugInformation> DebugInfo { get; }

        /// <summary>
        ///     local symbols
        /// </summary>
        IOption<LocalDebugSymbolMode> LocalSymbols { get; }

        /// <summary>
        ///     symbol definition info
        /// </summary>
        IOption<SymbolDefinitionInfo> SymbolDefinitions { get; }

        /// <summary>
        ///     symbol reference info
        /// </summary>
        IOption<SymbolReferenceInfo> SymbolReferences { get; }

        /// <summary>
        ///     imported data
        /// </summary>
        IOption<ImportGlobalUnitData> ImportedData { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options on a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}