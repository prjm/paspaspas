using System.Collections.Generic;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     metainformation about the compiled project
    /// </summary>
    public class MetaInformation {

        /// <summary>
        ///     list of external symblos
        /// </summary>
        public IList<ExternalSymbol> ExternalSymbols { get; }
            = new List<ExternalSymbol>();

        /// <summary>
        ///     list of header strings for c++
        /// </summary>
        public IList<HeaderString> HeaderStrings { get; }
            = new List<HeaderString>();

        /// <summary>
        ///     creates a new meta information option object
        /// </summary>
        /// <param name="baseOption"></param>
        public MetaInformation(MetaInformation baseOption) {
            Description = new DerivedValueOption<string>(baseOption?.Description);
            FileExtension = new DerivedValueOption<string>(baseOption?.FileExtension);
        }

        /// <summary>
        /// 
        /// </summary>
        public DerivedValueOption<string> Description { get; }

        /// <summary>
        ///     compiled outpout bilfe extension
        /// </summary>
        public DerivedValueOption<string> FileExtension { get; }

        /// <summary>
        ///     path resolver for includes
        /// </summary>
        public IncludeFilePathResolver IncludePathResolver { get; }


        /// <summary>
        ///     reset on new unit
        /// </summary>
        public void ResetOnNewUnit() {
            HeaderStrings.Clear();
        }

        /// <summary>
        ///     reset on new file
        /// </summary>
        internal void Clear() {
            Description.ResetToDefault();
            FileExtension.ResetToDefault();
            ExternalSymbols.Clear();
            HeaderStrings.Clear();
        }

        /// <summary>
        ///     register external symbol
        /// </summary>
        /// <param name="identiferName"></param>
        /// <param name="symbolName"></param>
        /// <param name="unionName"></param>
        public void RegisterExternalSymbol(string identiferName, string symbolName, string unionName) {
            ExternalSymbols.Add(new ExternalSymbol() {
                IdentifierName = identiferName,
                SymbolName = symbolName,
                UnionName = unionName
            });
        }

        /// <summary>
        ///     emit c++ header strings
        /// </summary>
        /// <param name="mode">emit mode</param>
        /// <param name="emitValue">emit value</param>
        public void HeaderEmit(HppEmitMode mode, string emitValue) {
            HeaderStrings.Add(new HeaderString() {
                Mode = mode,
                Value = emitValue
            });
        }
    }
}