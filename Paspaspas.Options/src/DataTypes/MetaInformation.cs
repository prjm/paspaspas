#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     meta information about the compiled project
    /// </summary>
    public class MetaInformation : IMetaOptions {

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
        ///     list of skipped includes
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
        public MetaInformation(IOptionSet parentOptions, IMetaOptions baseOption) {
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
            NativeIntegerSize = new DerivedValueOption<NativeIntSize>(baseOption?.NativeIntegerSize);
        }

        /// <summary>
        ///     project description
        /// </summary>
        public IOption<string> Description { get; }

        /// <summary>
        ///     compiled output file extension
        /// </summary>
        public IOption<string> FileExtension { get; }

        /// <summary>
        ///     path resolver for includes
        /// </summary>
        public IPathResolver IncludePathResolver { get; }

        /// <summary>
        ///     path resolve for resource files
        /// </summary>
        public ResourceFilePathResolver ResourceFilePathResolver { get; set; }

        /// <summary>
        ///     parent options
        /// </summary>
        public IOptionSet ParentOptions { get; }

        /// <summary>
        ///     lib prefix
        /// </summary>
        public IOption<string> LibPrefix { get; }

        /// <summary>
        ///     lib suffix
        /// </summary>
        public IOption<string> LibSuffix { get; }

        /// <summary>
        ///     lib version
        /// </summary>
        public IOption<string> LibVersion { get; }

        /// <summary>
        ///     required operating system version
        /// </summary>
        public IPEVersion PEOsVersion { get; }

        /// <summary>
        ///     required subsystem version
        /// </summary>
        public IPEVersion PESubsystemVersion { get; }

        /// <summary>
        ///     user version
        /// </summary>
        public IPEVersion PEUserVersion { get; }

        /// <summary>
        ///     resolver for directly linked input files
        /// </summary>
        public LinkedInputFileResolver LinkedFileResolver { get; }

        /// <summary>
        ///     native integer size
        /// </summary>
        public IOption<NativeIntSize> NativeIntegerSize { get; }

        /// <summary>
        ///     reset on new unit
        /// </summary>
        public void ResetOnNewUnit(ILogSource logSource) {

            foreach (var region in Regions) {
                logSource.LogError(MessageNumbers.PendingRegion, region);
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
        /// <param name="identifierName"></param>
        /// <param name="symbolName"></param>
        /// <param name="unionName"></param>
        public void RegisterExternalSymbol(string identifierName, string symbolName, string unionName) {

            if (string.IsNullOrWhiteSpace(identifierName))
                return;

            ExternalSymbols.Add(new ExternalSymbol(
                identifierName,
                symbolName,
                unionName
            ));
        }

        /// <summary>
        ///     emit c++ header strings
        /// </summary>
        /// <param name="mode">emit mode</param>
        /// <param name="emitValue">emit value</param>
        public void HeaderEmit(HppEmitMode mode, string emitValue)
            => HeaderStrings.Add(new HeaderString(mode, emitValue));

        /// <summary>
        ///     add a resource reference
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
        /// <param name="aliasName">name wrangling alias</param>
        public void AddObjectFileTypeName(string typeName, string aliasName) => ObjectFileTypeNames.Add(new ObjectFileTypeName(typeName, aliasName));

        /// <summary>
        ///     ignore a unit in header files in cpp
        /// </summary>
        /// <param name="unitName">unit name</param>
        public void AddNoInclude(string unitName)
            => NoIncludes.Add(unitName);

        /// <summary>
        ///     add a type name which is excluded from the generated header file
        /// </summary>
        /// <param name="noDefine"></param>
        public void AddNoDefine(DoNotDefineInHeader noDefine)
            => NoDefines.Add(noDefine);

        /// <summary>
        ///     a a linked file
        /// </summary>
        /// <param name="linkedFile"></param>
        public void AddLinkedFile(LinkedFile linkedFile)
            => LinkedFiles.Add(linkedFile);

        /// <summary>
        ///     test for open regions
        /// </summary>
        public bool HasRegions
            => Regions.Count > 0;

        /// <summary>
        ///     add a linked file
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fileReference"></param>
        public void AddLinkedFile(IFileReference basePath, IFileReference fileReference) {
            var resolvedFile = LinkedFileResolver.ResolvePath(basePath, fileReference);

            var linkedFile = new LinkedFile() {
                OriginalFileName = fileReference.Path,
                TargetPath = resolvedFile.TargetPath,
                IsResolved = resolvedFile.IsResolved
            };
            LinkedFiles.Add(linkedFile);
        }

        /// <summary>
        ///     add a resource reference
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fileReference"></param>
        /// <param name="rcFile"></param>
        public void AddResourceReference(IFileReference basePath, IFileReference fileReference, string rcFile) {
            var resolvedFile = ResourceFilePathResolver.ResolvePath(basePath, fileReference);

            var resourceReference = new ResourceReference() {
                OriginalFileName = fileReference.Path,
                TargetPath = resolvedFile.TargetPath,
                RcFile = rcFile,
                IsResolved = resolvedFile.IsResolved
            };

            ResourceReferences.Add(resourceReference);
        }

        /// <summary>
        ///     add a include
        /// </summary>
        public IFileReference AddInclude(IFileReference basePath, IFileReference fileName)
            => IncludePathResolver.ResolvePath(basePath, fileName).TargetPath;
    }
}