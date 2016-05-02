using PasPasPas.Options.Bundles;
using System.Collections.Generic;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     metainformation about the compiled project
    /// </summary>
    public class MetaInformation {

        /// <summary>
        ///     list of external symbols
        /// </summary>
        public IList<ExternalSymbol> ExternalSymbols { get; }
            = new List<ExternalSymbol>();

        /// <summary>
        ///     list of header strings for c++
        /// </summary>
        public IList<HeaderString> HeaderStrings { get; }
            = new List<HeaderString>();

        /// <summary>
        ///     list of resource references
        /// </summary>
        public IList<ResourceReference> ResourceReferences { get; }
            = new List<ResourceReference>();

        /// <summary>
        ///     creates a new meta information option object
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="parentOptions">parent options</param>
        public MetaInformation(OptionSet parentOptions, MetaInformation baseOption) {
            ParentOptions = parentOptions;
            Description = new DerivedValueOption<string>(baseOption?.Description);
            FileExtension = new DerivedValueOption<string>(baseOption?.FileExtension);
            IncludePathResolver = new IncludeFilePathResolver(parentOptions);
            ResourceFilePathResolver = new ResourceFilePathResolver(parentOptions);
            LibPrefix = new DerivedValueOption<string>(baseOption?.LibPrefix);
            LibSuffix = new DerivedValueOption<string>(baseOption?.LibSuffix);
            LibVersion = new DerivedValueOption<string>(baseOption?.LibVersion);
        }

        /// <summary>
        ///     project description
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
        ///     path resolve for resouce files
        /// </summary>
        public ResourceFilePathResolver ResourceFilePathResolver { get; set; }

        /// <summary>
        ///     parent options
        /// </summary>
        public OptionSet ParentOptions { get; }

        /// <summary>
        ///     lib prefix
        /// </summary>
        public DerivedValueOption<string> LibPrefix { get; }

        /// <summary>
        ///     lib suffix
        /// </summary>
        public DerivedValueOption<string> LibSuffix { get; }

        /// <summary>
        ///     lib version
        /// </summary>
        public DerivedValueOption<string> LibVersion { get; }

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
            LibPrefix.ResetToDefault();
            LibSuffix.ResetToDefault();
            LibVersion.ResetToDefault();
            ResourceReferences.Clear();
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

        /// <summary>
        ///     add a resoure reference
        /// </summary>
        /// <param name="resourceReference"></param>
        public void AddResourceReference(ResourceReference resourceReference) {
            ResourceReferences.Add(resourceReference);
        }
    }
}