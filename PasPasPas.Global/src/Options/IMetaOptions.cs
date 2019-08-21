using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     meta options
    /// </summary>
    public interface IMetaOptions {

        /// <summary>
        ///     description
        /// </summary>
        IOption<string> Description { get; }

        /// <summary>
        ///     file extension
        /// </summary>
        IOption<string> FileExtension { get; }

        /// <summary>
        ///     library prefix
        /// </summary>
        IOption<string> LibPrefix { get; }

        /// <summary>
        ///     library suffix
        /// </summary>
        IOption<string> LibSuffix { get; }

        /// <summary>
        ///     library version
        /// </summary>
        IOption<string> LibVersion { get; }

        /// <summary>
        ///     pe subsystem version
        /// </summary>
        IPEVersion PESubsystemVersion { get; }

        /// <summary>
        ///     pe operating system version
        /// </summary>
        IPEVersion PEOsVersion { get; }

        /// <summary>
        ///     pe user version
        /// </summary>
        IPEVersion PEUserVersion { get; }

        /// <summary>
        ///     native int size
        /// </summary>
        IOption<NativeIntSize> NativeIntegerSize { get; }

        /// <summary>
        ///     test if regions are open
        /// </summary>
        bool HasRegions { get; }

        /// <summary>
        ///     no defines
        /// </summary>
        IList<DoNotDefineInHeader> NoDefines { get; }

        /// <summary>
        ///     external symbols
        /// </summary>
        IList<ExternalSymbol> ExternalSymbols { get; }

        /// <summary>
        ///     header strings
        /// </summary>
        IList<HeaderString> HeaderStrings { get; }

        /// <summary>
        ///     linked files
        /// </summary>
        IList<LinkedFile> LinkedFiles { get; }

        /// <summary>
        ///     resource references
        /// </summary>
        IList<ResourceReference> ResourceReferences { get; }

        /// <summary>
        ///     no includes
        /// </summary>
        IList<string> NoIncludes { get; }

        /// <summary>
        ///     object file type names
        /// </summary>
        IList<ObjectFileTypeName> ObjectFileTypeNames { get; }

        /// <summary>
        ///     path resolver
        /// </summary>
        IPathResolver IncludePathResolver { get; }

        /// <summary>
        ///     register an external symbol
        /// </summary>
        /// <param name="identifierName"></param>
        /// <param name="symbolName"></param>
        /// <param name="unionName"></param>
        void RegisterExternalSymbol(string identifierName, string symbolName, string unionName);

        /// <summary>
        ///     set the header emit mode
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="emitValue"></param>
        void HeaderEmit(HppEmitMode mode, string emitValue);

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options on a new unit
        /// </summary>
        /// <param name="logSource"></param>
        void ResetOnNewUnit(ILogSource logSource);

        /// <summary>
        ///     add an object file type name
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="aliasName"></param>
        void AddObjectFileTypeName(string typeName, string aliasName);

        /// <summary>
        ///     add a unit not to be included
        /// </summary>
        /// <param name="unitName"></param>
        void AddNoInclude(string unitName);

        /// <summary>
        ///     stop region
        /// </summary>
        void StopRegion();

        /// <summary>
        ///     add a no define definition
        /// </summary>
        /// <param name="noDefine"></param>
        void AddNoDefine(DoNotDefineInHeader noDefine);

        /// <summary>
        ///     add a linked file
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fileReference"></param>
        void AddLinkedFile(IFileReference basePath, IFileReference fileReference);

        /// <summary>
        ///     start a region definition
        /// </summary>
        /// <param name="regionName"></param>
        void StartRegion(string regionName);

        /// <summary>
        ///     add a resource reference
        /// </summary>
        void AddResourceReference(IFileReference basePath, IFileReference fileReference, string rcFile);

        /// <summary>
        ///     add a include file
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fileName"></param>
        IFileReference AddInclude(IFileReference basePath, IFileReference fileName);
    }
}