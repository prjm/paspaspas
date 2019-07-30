using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     debug related options
    /// </summary>
    public class DebugOptions : IDebugOptions {

        /// <summary>
        ///     create a new set of debug options
        /// </summary>
        /// <param name="baseOptions"></param>
        public DebugOptions(IDebugOptions baseOptions) {
            Assertions = new DerivedValueOption<AssertionMode>(baseOptions?.Assertions);
            DebugInfo = new DerivedValueOption<DebugInformation>(baseOptions?.DebugInfo);
            ImportedData = new DerivedValueOption<ImportGlobalUnitData>(baseOptions?.ImportedData);
            LocalSymbols = new DerivedValueOption<LocalDebugSymbolMode>(baseOptions?.LocalSymbols);
            SymbolReferences = new DerivedValueOption<SymbolReferenceInfo>(baseOptions?.SymbolReferences);
            SymbolDefinitions = new DerivedValueOption<SymbolDefinitionInfo>(baseOptions?.SymbolDefinitions);
        }

        /// <summary>
        ///     Assertion mode
        /// </summary>
        public IOption<AssertionMode> Assertions { get; }

        /// <summary>
        ///     debug info
        /// </summary>
        public IOption<DebugInformation> DebugInfo { get; }

        /// <summary>
        ///     option for global unit access
        /// </summary>
        public IOption<ImportGlobalUnitData> ImportedData { get; }

        /// <summary>
        ///     local symbols flag
        /// </summary>
        public IOption<LocalDebugSymbolMode> LocalSymbols { get; }

        /// <summary>
        ///     flag go generate symbol reference information
        /// </summary>
        public IOption<SymbolReferenceInfo> SymbolReferences { get; }

        /// <summary>
        ///     flag to generate symbol definition information
        /// </summary>
        public IOption<SymbolDefinitionInfo> SymbolDefinitions { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            Assertions.ResetToDefault();
            DebugInfo.ResetToDefault();
            ImportedData.ResetToDefault();
            LocalSymbols.ResetToDefault();
            SymbolReferences.ResetToDefault();
            SymbolDefinitions.ResetToDefault();
        }

        /// <summary>
        ///     reset options
        /// </summary>
        public void ResetOnNewUnit() {
            Assertions.ResetToDefault();
            ImportedData.ResetToDefault();
        }
    }
}
