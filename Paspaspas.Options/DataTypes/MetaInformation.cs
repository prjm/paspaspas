using PasPasPas.Infrastructure.Log;
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
        ///     object file type name information
        /// </summary>
        public IList<ObjectFileTypeName> ObjectFileTypeNames { get; }
            = new List<ObjectFileTypeName>();

        /// <summary>
        ///     list of skipped inludes
        /// </summary>
        public IList<string> NoIncludes { get; }
            = new List<string>();

        /// <summary>
        ///     list of skipped header definition
        /// </summary>
        public IList<DoNotDefineInHeader> NoDefines { get; }
            = new List<DoNotDefineInHeader>();

        /// <summary>
        ///     current regions
        /// </summary>
        public Stack<string> Regions { get; }
            = new Stack<string>();

        /// <summary>
        ///     list of directly linked files
        /// </summary>
        public IList<LinkedFile> LinkedFiles { get; }
            = new List<LinkedFile>();

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
            PEOsVersion = new PEVersion(baseOption?.PEOsVersion);
            PESubsystemVersion = new PEVersion(baseOption?.PESubsystemVersion);
            PEUserVersion = new PEVersion(baseOption?.PEUserVersion);
            LinkedFileResolver = new LinkedInputFileResolver(parentOptions);
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
        ///     required os version
        /// </summary>
        public PEVersion PEOsVersion { get; set; }

        /// <summary>
        ///     required subsystem version
        /// </summary>
        public PEVersion PESubsystemVersion { get; }

        /// <summary>
        ///     user version
        /// </summary>
        public PEVersion PEUserVersion { get; }

        /// <summary>
        ///     resolver for directly linked input files
        /// </summary>
        public LinkedInputFileResolver LinkedFileResolver { get; }

        /// <summary>
        ///     reset on new unit
        /// </summary>
        public void ResetOnNewUnit(LogSource logSource) {

            foreach (var region in Regions) {
                logSource.Error(OptionSet.PendingRegion, region);
            }

            HeaderStrings.Clear();
            Regions.Clear();
            NoIncludes.Clear();
            NoDefines.Clear();
            NoIncludes.Clear();
        }

        /// <summary>
        ///     reset on new file
        /// </summary>
        public void Clear() {
            Description.ResetToDefault();
            FileExtension.ResetToDefault();
            ExternalSymbols.Clear();
            HeaderStrings.Clear();
            LibPrefix.ResetToDefault();
            LibSuffix.ResetToDefault();
            LibVersion.ResetToDefault();
            ResourceReferences.Clear();
            Regions.Clear();
            PEOsVersion.Clear();
            PESubsystemVersion.Clear();
            PEUserVersion.Clear();
            ObjectFileTypeNames.Clear();
            NoIncludes.Clear();
            NoDefines.Clear();
            LinkedFiles.Clear();
        }

        /// <summary>
        ///     register external symbol
        /// </summary>
        /// <param name="identiferName"></param>
        /// <param name="symbolName"></param>
        /// <param name="unionName"></param>
        public void RegisterExternalSymbol(string identiferName, string symbolName, string unionName) {

            if (string.IsNullOrWhiteSpace(identiferName))
                return;

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
        public void HeaderEmit(HppEmitMode mode, string emitValue) => HeaderStrings.Add(new HeaderString() {
            Mode = mode,
            Value = emitValue
        });

        /// <summary>
        ///     add a resoure reference
        /// </summary>
        /// <param name="resourceReference"></param>
        public void AddResourceReference(ResourceReference resourceReference) => ResourceReferences.Add(resourceReference);

        /// <summary>
        ///     start of region
        /// </summary>
        /// <param name="regionName">optional region AME</param>
        public void StartRegion(string regionName) => Regions.Push(regionName ?? string.Empty);

        /// <summary>
        ///     end of region
        /// </summary>
        public void StopRegion() => Regions.Pop();

        /// <summary>
        ///     add object file type name
        /// </summary>
        /// <param name="typeName">type name</param>
        /// <param name="aliasName">name wranglin alias</param>
        public void AddObjectFileTypeName(string typeName, string aliasName) => ObjectFileTypeNames.Add(new ObjectFileTypeName(typeName, aliasName));

        /// <summary>
        ///     ignore a unit in header files in cpp
        /// </summary>
        /// <param name="unitName">unit name</param>
        public void AddNoInclude(string unitName) => NoIncludes.Add(unitName);

        /// <summary>
        ///     add a type name which is excluded from the generated header file
        /// </summary>
        /// <param name="typeName">type name</param>
        /// <param name="nameInHpp">tpye name used in headers</param>
        /// <param name="typeNameInUnion">type name used in unions</param>
        public void AddNoDefine(string typeName, string nameInHpp, string typeNameInUnion) => NoDefines.Add(new DoNotDefineInHeader(typeName, nameInHpp, typeNameInUnion));

        /// <summary>
        ///     a a linked file
        /// </summary>
        /// <param name="linkedFile"></param>
        public void AddLinkedFile(LinkedFile linkedFile) => LinkedFiles.Add(linkedFile);
    }
}