using System.Collections.Generic;

namespace PasPasPas.Api.Options {

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
        ///     reset on new unit
        /// </summary>
        public void ResetOnNewUnit() {
            //..
        }

        /// <summary>
        ///     reset on new file
        /// </summary>
        internal void Clear() {
            Description.ResetToDefault();
            FileExtension.ResetToDefault();
            ExternalSymbols.Clear();
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
    }
}